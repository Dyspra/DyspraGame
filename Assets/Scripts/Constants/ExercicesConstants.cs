using System.Collections.Generic;

namespace Constants
{
    public static class ExerciseConstants
    {
        public enum E_QuitReason
        {
            Quit,
            Complete,
            Failed
        }

        public class Exercise
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Scene { get; set; }
            public string Image { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// List of all exercises
        /// </summary>
        public static Dictionary<string, Exercise> Exercises = new Dictionary<string, Exercise>()
        {
            {"1", new Exercise {
                Id = "1",
                Name = "Foire aux ballons 2",
                Scene = "BalloonFair",
                Image = "balloon_fair.png",
                Description = ""
            }},
            {"2", new Exercise {
                Id = "2",
                Name = "Ballet des méduses",
                Scene = "Exercice2",
                Image = "image exo2.png",
                Description = ""
            }},
            {"3", new Exercise {
                Id = "3",
                Name = "Chamboule tout",
                Scene = "ChambouleTout",
                Image = "image exo3.png",
                Description = ""
            }},
            {"4", new Exercise {
                Id = "4",
                Name = "Foire aux ballons 1",
                Scene = "EasyBalloonFair",
                Image = "image easy.png",
                Description = ""
            }},
            {"5", new Exercise
            {
                Id = "5",
                Name = "Cuisine à sandwichs",
                Scene = "Exercice4",
                Image = "imageexo4.png",
                Description = ""
            } }
        };
    }
}