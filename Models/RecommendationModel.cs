using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace RecommendationSystem.Models
{
    public class RecommendationModel
    {
        private MLContext _mlContext;
        private ITransformer _model;

        public RecommendationModel()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(IEnumerable<Interaction> trainingData)
        {
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

        public IEnumerable<(int itemId, float score)> GetRecommendations(int userId, int count)
        {
            var allItems = Enumerable.Range(1, 1000); // Assume we have 1000 items
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