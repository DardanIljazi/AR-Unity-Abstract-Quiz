public abstract class ApiModel : AbstractQuizzStructure
{
    public Quizzes quizzes;
    public Quizz quizz;

    public Questions questions;
    public Question question;

    public Answers answers;
    public Answer answer;

    public ApiModel()
    {
        this.quizzes    = new QuizzesStructureInAPI();
        this.quizz      = new QuizzStructureInAPI();

        this.questions  = new QuestionsStructureInAPI();
        this.question   = new QuestionStructureInAPI();

        this.answers    = new AnswersStructureInApi();
        this.answer     = new AnswerStructureInApi();
    }

    public class QuizzesStructureInAPI : Quizzes
    {
        // Will Serialize raw data (json) to the Quizzes class
        public override Quizzes SerializeJsonToQuizzes(string json)
        {
            if (json == null || json.Length == 0)
                return null;

            APIQuizzes data = JsonUtility.FromJson<APIQuizzes>(json);

            if (data == null)
                return null;

            data.MapAPIValuesToAbstractClass();

            return data;
        }
    }

    public class QuizzStructureInAPI : Quizz
    {

    }

    public class QuestionsStructureInAPI : Questions
    {

    }

    public class QuestionStructureInAPI : Question
    {

    }

    public class AnswersStructureInApi : Answers
    {

    }

    public class AnswerStructureInApi : Answer
    {

    }
}