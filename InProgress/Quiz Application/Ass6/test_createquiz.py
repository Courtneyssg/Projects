#cssg02@mun.ca - 200917250
import unittest
from datetime import date
from CreateQuiz import Create
from Question import Question
from quiz import Quiz
from Course import Course
from temp_persist import Persist
class TestCreateQuiz(unittest.TestCase):

    def setUp(self):
        self.created = Create("test.dat")

        self.course = Course("2005", "brown@mun.ca", "Abc_123", ["cssg02@mun.ca"])
        self.course2 = Course("2006", "rod@mun.ca", "ABc123_", ["cssg02@mun.ca"])
        self.quiz = Quiz("quiz1", 3, date(2019, 3, 10), date(2019, 4, 3))
        self.quiz2 = Quiz("quiz5", 1, date(2019, 3, 10), date(2019, 4, 20))
        self.question = Question("2005", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5)
        self.question2 = Question("2005", "Question2", ["a", "b", "c", "d"], ["c"], 1)


    def test_add_question(self):
        """Checks if question is properly added"""
        self.created.add_question("2005", "quiz1", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5)
        self.assertEqual(self.question.question, self.created.pull_question("2005", "quiz1", "Question1"))

    def test_add_question_error(self):
        """checks if KeyError is raised for dictionary key"""
        with self.assertRaises(KeyError):
            self.created.add_question(2005, "quiz1", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5)


    def test_delete_question(self):
        """tests if question was deleted"""
        #self.created.add_question("2005", "quiz1", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5)
        self.assertTrue(self.created.add_question("2005", "quiz1", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5), msg="question added")
        #self.created.delete_question("2005", "quiz1", "Question1")
        self.assertTrue(self.created.delete_question("2005", "quiz1", "Question1"), msg="question deleted")

    def test_delete_question_error(self):
        """tests if key error is raised when specifying dictionary key"""
        with self.assertRaises(KeyError):
            self.created.delete_question(2005, "quiz1", "Questions1")

    def test_make_duplicate(self):
        """Tests if a duplicate is made in quiz_questions"""
        self.assertTrue(self.created.add_question("2005", "quiz1", "Question1",["a", "b", "c", "d"], ["a", "b"],5), msg="question added")
        self.assertTrue(self.created.make_duplicate("2005", "quiz1", "Question1"), msg="duplicate made")

    def test_make_duplicate_error(self):
        """Tests if keyerror is raised when entering key"""
        with self.assertRaises(KeyError):
            self.created.make_duplicate(2005, "quiz1", "Question1")

    def test_modify_question(self):
        """Tests if modify question changed the question contents"""
        self.assertTrue(self.created.add_question("2005", "quiz1", "Question1",["a", "b", "c", "d"], ["a", "b"], 5), msg="question added")
        self.assertTrue(self.created.modify_question("2005", "quiz1", "Question1", "Question2",["a", "b", "c", "d"], ["a", "b"], 5), msg="question modified")

    def test_modify_question_error(self):
        """Tests if keyerror is raised when entering dictionary key"""
        with self.assertRaises(KeyError):
            self.created.modify_question(2005, "quiz1", "Question1", "Question2", ["a", "b", "c", "d"], ["a", "b"], 5)


    def test_copy_question(self):
        self.assertTrue(self.created.add_question("2005", "quiz1", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5), msg="question added")
        self.assertTrue(self.created.copy_question("2005", "quiz1", "Question1"), msg="question copied")

    def test_copy_question_error(self):
        with self.assertRaises(KeyError):
            self.created.copy_question(2005, "quiz1", "Question1")

    def test_calculate_worth_error(self):
        """Tests if question values are summed up properly"""
        self.assertTrue(self.created.add_question("2005", "quiz1", "Question1",["a", "b", "c", "d"], ["a", "b"],285), msg="question added")
        self.assertEqual(self.created.calculate_worth("2005", "quiz1"), 290) #290 because question worth 5 manually added in persist along with above 285
    def test_calculate_worth(self):
        """Tests if quiz value is being stored quiz"""
        with self.assertRaises(KeyError):
            self.created.calculate_worth(2005, "quiz1")

    def test_add_quiz(self):
        """Tests if a quiz was added to course through shelve"""
        self.created.add_quiz("2005", "quiz5", 1, date(2019, 3, 10), date(2019, 4, 20))
        self.assertEqual(self.quiz2.quiz_name, self.created.pull_quiz("2005", "quiz5").quiz_name)

    def test_add_quiz_error(self):
        """Tests key error is raised if key entered incorrectly"""
        with self.assertRaises(KeyError):
            self.created.add_quiz(2005, "quiz6", 1, date(2019, 3, 10), date(2019, 4, 20))


    def test_add_to_bank(self):
        """tests if question is added to question bank"""
        self.created.add_to_bank("2005", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5)
        self.assertEqual(self.question.question,self.created.pull_question_bank("2005", "Question1"))

    def test_add_to_bank_error(self):
        """tests that key error was raised"""
        with self.assertRaises(KeyError):
            self.created.add_to_bank(2005, "Question1", ["a", "b", "c", "d"], ["a", "b"], 5)

    def test_modify_bank_questions(self):
        """tests that question in question bank was modified"""
        self.assertTrue(self.created.add_to_bank("2005", "Question1", ["a", "b", "c", "d"], ["a", "b"], 5))
        self.assertTrue(self.created.modify_bank_questions("2005", "Question1","Question2",["a", "b", "c", "d"],["a", "b"], 5),msg="question modified")

    def test_modify_bank_questions_error(self):
        """tests that a key error is raised"""
        with self.assertRaises(KeyError):
            self.created.modify_bank_questions(2005, "Question1","Question2", ["a", "b", "c", "d"],["a", "b"],5)

    def test_modify_quiz(self):
        """tests if quiz qas modified"""
        self.created.add_quiz("2005", "quiz5", 1, date(2019, 3, 10), date(2019, 4, 20))
        self.assertTrue(self.created.modify_quiz("2005", "quiz5", 4, date(2019, 3, 10), date(2019, 4, 20)))

    def test_modify_quiz_error(self):
        """tests that key error was raised"""
        with self.assertRaises(KeyError):
            self.created.modify_quiz(2005, "quiz5", 4, date(2019, 3, 10), date(2019, 4, 20))

    def test_copy_quiz(self):
        """tests if a quiz copy was made"""
        self.created.add_quiz("2005", "quiz5", 1, date(2019, 3, 10), date(2019, 4, 20))
        self.assertTrue(self.created.copy_quiz("2005", "quiz5", "quiz10"))

    def test_copy_quiz_error(self):
        with self.assertRaises(KeyError):
            self.created.copy_quiz(2005, "quiz5", "quiz10")

    def test_delete_quiz(self):
        """tetsts if a quiz was deleted"""
        self.created.add_quiz("2005", "quiz5", 1, date(2019, 3, 10), date(2019, 4, 20))
        self.assertTrue(self.created.delete_quiz("2005", "quiz5"))

    def test_delete_quiz_error(self):
        with self.assertRaises(KeyError):
            self.created.delete_quiz(2005, "quiz5")


if __name__ == "__main__":
    unittest.main(verbosity=2)

