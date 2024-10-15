using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSystem.Models
{
    public class RecommendationModel
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private readonly AppDbContext _dbContext;

        public RecommendationModel(AppDbContext dbContext)
        {
            _mlContext = new MLContext();
            _dbContext = dbContext;
        }

        public async Task TrainModelAsync()
        {
            var trainingData = await _dbContext.Interactions.ToListAsync();
            var trainData = _mlContext.Data.LoadFromEnumerable(trainingData.Select(i => new MovieRating
            {
                userId = (uint)i.UserId,
                movieId = (uint)i.ItemId,
                Label = i.Rating
            }));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userId",
                MatrixRowIndexColumnName = "movieId",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainer = _mlContext.Recommendation().Trainers.MatrixFactorization(options);
            _model = trainer.Fit(trainData);
        }

        public async Task<IEnumerable<(int itemId, float score)>> GetRecommendationsAsync(int userId, int count)
        {
            if (_model == null)
            {
                await TrainModelAsync();
            }

            var allItems = await _dbContext.Items.Select(i => i.Id).ToListAsync();
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(_model);

            return allItems
                .Select(itemId => (itemId, score: predictionEngine.Predict(new MovieRating { userId = (uint)userId, movieId = (uint)itemId }).Score))
                .OrderByDescending(x => x.score)
                .Take(count);
        }

        private class MovieRating
        {
            public float Label;
            [KeyType(count: 10000)]
            public uint userId;
            [KeyType(count: 10000)]
            public uint movieId;
        }

        private class MovieRatingPrediction
        {
            public float Label;
            public float Score;
        }
    }
}
