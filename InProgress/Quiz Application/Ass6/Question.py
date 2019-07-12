#cssg02@mun.ca - 200917250
class Question:
    """Creates a question to be stored in a created quiz"""

    def __init__(self, course, question, answers, correct_answers, value):
        """Stores question information
        Parameters:
            question : string - question to ask
            answers : list of strings - list of options for question
            correct_answers: list of strings - list of correct answers
            value : int - what question is worth in points
            """
        self.course = course
        self.question = question
        self.answers = answers
        self.correct_answers = correct_answers
        self.value = value

