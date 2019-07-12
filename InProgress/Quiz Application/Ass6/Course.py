#cssg02@mun.ca - 200917250

class Course:
    """This class will hold all created quizzes
    along with the course, and professor"""

    def __init__(self, course_number, instructor, password, students):
        """Constructor holds course number, instructors login info,
        a list of student login info, a bank of quizzes associated with
        the course, and a question bank that holds all questions created
        for this course"""
        self.course_number = course_number
        self.instructor = instructor
        self.instructor_password = password
        self.students = students
        self.quizzes = {}
        self.question_bank = []