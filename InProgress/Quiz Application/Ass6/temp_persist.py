#cssg02@mun.ca - 200917250
import shelve
from datetime import date
from Course import Course
from quiz import Quiz
from Question import Question

class Persist:

    def __init__(self, db_name):

        self.course_shelve = shelve.open(db_name, writeback=True)
        "BELOW CODE ADDED FOR TESTING PURPOSE IN CREATEQUIZ MAIN"
        course = Course("2005", "brown@mun.ca", "12345", ["cssg02@mun.ca"])
        course.quizzes["quiz1"] = Quiz("quiz1", 3, date(2019, 3, 10), date(2019, 4, 3))
        course.quizzes['quiz1'].quiz_questions.append(Question("2005", "Q1?", ["a", "b", "c"], ["a"], 5))
        self.course_shelve["2005"] = course

    def get_course(self, course):
        return self.course_shelve[course]

    def set_course(self, course, courseobj):
        self.course_shelve[course] = courseobj

    def get_quiz(self, course, quiz):
        return self.course_shelve[course].quizzes[quiz]

    def set_quiz(self, course, quiz, quizobj):
        self.course_shelve[course].quizzes[quiz] = quizobj

    def get_question_bank(self, course):
        return self.course_shelve[course].question_bank

    def set_question_bank(self, course, bank):
        self.course_shelve[course].question_bank = bank

    def get_courses(self):
        """Able to retrieve the course keys

        Returns:
            self.myshelve.keys()
        """
    def get_course_obj(self, course):
        """Able to retrieve course objects

        Returns:
            self.myshelve[course]
            """

    def add_course(self, course):

        """Add course object to the shelve with course name as key

        Parameters:
            course : Course Object - Instance of a created course
        """
    def close(self):
        """Closes the shelve with sync and close"""