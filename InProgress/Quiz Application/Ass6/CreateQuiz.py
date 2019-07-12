#cssg02@mun.ca - 200917250
from Question import Question
from Course import Course
from temp_persist import Persist
from datetime import date
from quiz import Quiz
from Login import *
"""New Quiz Creation

    Classes:
        Create - provides a quiz object for Course class
        
    Methods:
        add_question - add question to questions list for quiz
        delete_question - delete question from quiz under construction
        make_duplicate - creates copy of a question and appends to list
        modify_question - modify question from quiz under construction
        copy_question - create copy of the specified question
        calculate_worth - calculates the total of all question values
    """


class Create:
    """This class creates a quiz"""

    def __init__(self, db_name):

        self.db = Persist(db_name)

    def add_question(self, course, quiz, question, answers, correct_answers, value):

        """Creates and adds a new question to quiz_questions list

        Parameters:
            course : string - course in which quiz is located
            quiz : string - quiz to add question to
            question : string - question to be asked
            answers : list of strings - multiple choice options
            correct_answers : list of string - correct answer(s)
            value : int - how many points the question is worth

        Raises:
            KeyError - If course isn't correct type for key
            TypeError - If any parameters are not their listed type

        Returns:
            True - If question was added
            False - If question was not added
        """
        result = False
        bankresult = False
        if type(course) != str:
            raise KeyError("Wrong Type")

        if type(quiz) != str or type(question) !=\
                str or type(answers) != list or type(correct_answers) \
                != list or type(value) != int:
            raise TypeError("Wrong Type")
        for i in answers:
            if type(i) != str:
                raise TypeError("You entered something wrong")
        for i in correct_answers:
            if type(i) != str:
                raise TypeError("You entered something wrong")

        q = Question(course, question, answers, correct_answers, value)
        qu = self.db.get_quiz(course,quiz)
        qu.quiz_questions.append(q)
        bank = self.db.get_question_bank(course)
        bank.append(q)
        self.db.set_quiz(course, quiz, qu)
        self.db.set_question_bank(course, bank)
        for i in qu.quiz_questions:
            if i.question == q.question:
                result = True
        for i in bank:
            if i.question == q.question:
                bankresult = True

        if result and bankresult:
            return True
        else:
            return False

    def delete_question(self, course, quiz, question):
        """ Deletes a question from the quiz_questions list

        Look for specified question within specified course and quiz

        Parameters:
            course : string - course in which quiz is stored
            quiz : string - quiz in which question is stored
            question : string - question in question object

        Raises:
            KeyError if course isn't correct type for key

        Returns:
            True - If question was removed
            False - If question wasn't removed
        """
        if type(course) != str:
            raise KeyError("Wrong Type For Key")

        result = False
        qu = self.db.get_quiz(course,quiz)
        count = len(qu.quiz_questions)
        for q in qu.quiz_questions:
            if q.question == question:
                qu.quiz_questions.remove(q)
                break #prevent duplicate from getting deleted
        self.db.set_quiz(course, quiz, qu)
        count2 = len(qu.quiz_questions)
        if count - 1 == count2:
            result = True
        return result

    def make_duplicate(self, course, quiz, question):
        """A copy of the specified question is made in the list

        Will iterate through quiz_questions and find specified question,
        and append a copy at the end of the list

        Parameters:
            course : string - course in which quiz is stored
            quiz : string - quiz in which question is stored
            question : string - question that needs to be copied

        Raises:
            KeyError if wrong type is entered for dictionary key

        Returns:
            True - If question was duplicated
            False - If copy does not exist"""

        if type(course) != str:
            raise KeyError("Wrong type for key")
        result = False
        count = 0
        qu = self.db.get_quiz(course, quiz)
        for q in qu.quiz_questions:
            if q.question == question:
                qu.quiz_questions.append(q)
                break
        for i in qu.quiz_questions:
            if i.question == question:
                count += 1
        if count == 2:
            result = True
        self.db.set_quiz(course, quiz, qu)
        return result


    def modify_question(self, course, quiz, question, newquestion, newanswers, newcorrect, newval):
        """Modify exiting quiz question

        Choose a question to modify and provide the modifications

        Parameters:
            course : string - course that holds the quiz
            quiz : string - quiz that holds the question
            question : string - question to be modified
            newquestion : string - edited question
            newanswers : list of strings - edited answers
            newcorrect : list of strings - edited correct answers
            newval : int - edited question value

        Raises TypeError if any paramaters are not correct type

        Returns:
            True - If question was modified in any way
            False - If question remains unchanged"""

        if type(course) != str:
            raise KeyError("Not correct type for dictionary key")
        result = False
        qu = self.db.get_quiz(course, quiz)
        for q in qu.quiz_questions:
            if q.question == question:
                que = q.question
                ans = q.answers
                cans = q.correct_answers
                val = q.value
                q.question = newquestion
                q.answers = newanswers
                q.correct_answers = newcorrect
                q.value = newval
                if que != newquestion or val != newval:
                    result = True
        self.db.set_quiz(course, quiz, qu)
        return result

    def copy_question(self, course, quiz, question):
        """Copies a question from the question bank

        adds the specified question from the question bank to the
        specified quiz

        Parameters:
            course : string - course in which the quiz is stored
            quiz : string - quiz you want to add question to
            question : string - question to copy from bank to quiz

        Raises KeyError if dictionary key is incorrect type

        Returns:
            True - If question was successfully added from question bank
            False - If question was not put into quiz from question bank
            """
        if type(course) != str:
            raise KeyError("Not correct type for dictionary key")
        result = False
        bank = self.db.get_question_bank(course)
        qu = self.db.get_quiz(course, quiz)
        for q in bank:
            if q.question == question:
                qu.quiz_questions.append(q)
        self.db.set_quiz(course, quiz, qu)
        if question == qu.quiz_questions[len(qu.quiz_questions) - 1].question:
            result = True
        return result

    def calculate_worth(self, course, quiz):
        """calculates the value added to each question

        Will iterate through quiz_questions and sum up the values for
        all quiz questions and store the amount in .total_worth

        Parameters:
            course : string - the course where quiz is stored
            quiz : string - name of quiz to sum up total for
        Raises:
            KeyError if course isn't correct type for dictionary key
        Returns:
            total worth of quiz
            """
        if type(course) != str:
            raise KeyError("Not correct type for key")
        total = 0
        qu = self.db.get_quiz(course, quiz)
        for q in qu.quiz_questions:
            total = total + q.value
        return total

    def add_quiz(self, course, quiz_name, attempts, start_time, end_time):
        """Creates an instance of Quiz and adds it to the quiz
        dictionary associated with a course

        Parameters:
            course : string - course where quiz is to be stored
            quiz_name : string - name of the quiz
            attempts : int - number of allowed attempts for quiz
            start_time : datetime.date - start date for quiz
            end_time : datetime.date - end date for quiz
        Raises:
            TypeError - if parameters are not proper type
            KeyError - if Key (course) is not proper type

        Returns:
            True - If new quiz was added to quizzes
            False - If new quiz was not added to quizzes
        """
        if type(course) != str:
            raise KeyError("Wrong type")

        result = False
        newquiz = Quiz(quiz_name, attempts, start_time, end_time)
        qu = self.db.get_course(course)
        oldquizzes = len(qu.quizzes)
        qu.quizzes[quiz_name] = newquiz
        self.db.set_quiz(course, quiz_name, newquiz)
        newquizzes = len(qu.quizzes)

        if qu.quizzes[quiz_name].quiz_name == quiz_name and oldquizzes == newquizzes-1:
            result = True
        return result


    def add_to_bank(self, course, question, answers, correctanswers, value):
        """creates a question instance to add to the question bank
        without adding it to a quiz

        Parameters:
            course : string - to specify the course question bank
            question : string - the question to be added
            answers : list of strings - the options for question
            correctanswers : list of stings - correct answers
            value : int - what the question is worth

        Raises: KeyError if course isn't string for dictionary key
        Returns:
            True - If question was added to question bank
            False - If question was not added to question bank
        """

        if type(course) != str:
            raise KeyError("Not correct type for dictionary key")
        result = False
        ques = Question(course, question, answers, correctanswers, value)
        bank = self.db.get_question_bank(course)
        bank.append(ques)
        self.db.set_question_bank(course, bank)

        for q in bank:
            if q.question == question:
                result = True

        return result

    def modify_bank_questions(self, course, question, newquestion, ans, correctans, val):
        """Modifies a specified question in the question bank

        Accesses question_bank list and makes modifications to the
        specified question

        Parameters:
            course : string - specifies which course to access
            question : string - question to modify in the bank

        Raises: KeyError if course is wrong key type

        Returns:
            True - If question was modified in questionbank
            False - If question was not modified in questionbank
            """
        if type(course) != str:
            raise KeyError("Wrong type for dictionary key")
        result = False
        bank = self.db.get_question_bank(course)
        for q in bank:
            if q.question == question:
                q.answers = ans
                q.correct_answers = correctans
                q.value = val
                q.question = newquestion
        self.db.set_question_bank(course, bank)
        for i in bank:
            if i.question == newquestion:
                result = True
        return result


    def modify_quiz(self, course, searchquiz, newattempts, newstart, newend):
        """Search for a quiz in the quiz dictionary to edit

        Accesses quizzes associated with specified course and lets the
        user edit already existing quiz parameters. Use the method
        modify_question to modify a question in a quiz

        Parameters:
            course : string - quiz
            searchquiz : string - quiz name to search for
            newattempts : int - new number of attempts to set
            newstart : datetime.date - changes start time
            newend : datetime.date - changes end time

        Raises: KeyError if course isn't propery dictionary key type
        Returns:
            True - If quiz was modified at all
            False - If quiz was not modified

            """
        if type(course) != str:
            raise KeyError("Not proper key type")
        result = False
        count = 0
        qu = self.db.get_course(course)
        for q in qu.quizzes:
            count +=1
            if q == searchquiz:
                a = qu.quizzes[searchquiz].attempts
                s = qu.quizzes[searchquiz].start_time
                e = qu.quizzes[searchquiz].end_time
                qu.quizzes[searchquiz].attempts = newattempts
                qu.quizzes[searchquiz].start_time = newstart
                qu.quizzes[searchquiz].end_time = newend
                if a != newattempts or s != newstart or e != newend:
                    result = True
            elif count == len(qu.quizzes):
                print("Could not find quiz")
        self.db.set_course(course, qu)
        return result


    def copy_quiz(self, course, searchquiz, newquiz):
        """Make a copy of a quiz under a new quiz name


        Will search for specified quiz in the quizzes dictionary, and
        make a copy of that quiz under a new quiz name and add it to
        the quizzes dictionary

        Parameters:
            course : string - name of course to search under
            searchquiz : string - name of quiz to search for
            newquiz : string - name of new quiz created

        Raises KeyError if course is't correct type for key

        Returns:
            True - If a quiz was copied and added to quizzes
            False - If q quiz was not copied and added
            """
        if type(course) != str:
            raise KeyError("Not correct type for key")

        result = False
        c = self.db.get_course(course)
        oldlength = len(c.quizzes)
        for q in c.quizzes:
            if q == searchquiz:
                c.quizzes[newquiz] = c.quizzes[searchquiz]
                c.quizzes[newquiz].quiz_name = newquiz
                break

        self.db.set_course(course, c)
        newlength = len(c.quizzes)
        for q in c.quizzes:
            if q == newquiz and oldlength == newlength-1:
                result = True
        return result

    def delete_quiz(self, course, quiz):
        """delete existing quiz

        Searches for specified quiz in the quizzes dictionary and if
        found deletes the quiz

        Parameters:
            course : string - course in which quiz is stored
            quiz : string - name of quiz to delete

        raises KeyError if parameters are not correct type

        Returns:
            True - if quiz was deleted
            False - if quiz was not deleted
            """
        if type(course) != str:
            raise KeyError("Not correctkey")
        result = False
        c = self.db.get_course(course)
        oldlength = len(c.quizzes)
        for q in c.quizzes:
            if q == quiz:
                c.quizzes.pop(q)
                break
        self.db.set_course(course, c)
        newlength = len(c.quizzes)

        for q in c.quizzes:
            if q != quiz and newlength == oldlength-1:
                result = True
        return result

    def pull_quiz(self, course, quiz):
        """for use with unittest to test if storing is working"""
        q = self.db.get_quiz(course, quiz)
        return q

    def pull_question(self, course, quiz, question):
        """for use with unittest to test if storing is working"""
        q = self.db.get_quiz(course, quiz)
        for qu in q.quiz_questions:
            if qu.question == question:
                return qu.question

    def pull_question_bank(self, course, question):
        q = self.db.get_question_bank(course)
        for i in q:
            if i.question == question:
                return i.question



if __name__ == "__main__":

    create = Create('test,dat')

    print(create.add_question("2005", "quiz1", "Q2?", ["a", "b", "c", "d"], ["a", "b"], 5))
    print("Expecting 2 questions")
    for i in create.db.course_shelve['2005'].quizzes['quiz1'].quiz_questions:
        print(i.question)
    print()

    print(create.make_duplicate("2005", "quiz1", "Q1?"))
    print("expecting 3 questions q1 duplicated")
    for i in create.db.course_shelve['2005'].quizzes['quiz1'].quiz_questions:
        print(i.question)
    print()

    print(create.delete_question("2005", "quiz1", "Q1?"))
    print("expecting first q1 deleted, q2 and q1 remaining")
    for i in create.db.course_shelve['2005'].quizzes['quiz1'].quiz_questions:
        print(i.question)
    print()

    print(create.copy_question("2005", "quiz1", "Q2?"))
    print("expecting two q2's, extra added from questionbank")
    for i in create.db.course_shelve['2005'].quizzes['quiz1'].quiz_questions:
        print(i.question)
    print()

    print("Expecting 15")
    print(create.calculate_worth("2005", "quiz1"))

    print()
    print("Before adding quiz: ")
    for i in create.db.course_shelve["2005"].quizzes:
        print(i)
    print(create.add_quiz("2005", "quiz2", 3, date(2019,3,10), date(2019,4,3)))
    print("Expecting 2 quizzes in 2005: ")
    for i in create.db.course_shelve["2005"].quizzes:
        print(i)
    print()

    for i in create.db.course_shelve['2005'].question_bank:
        print("question bank before adding new question: ", i.question)
    print()

    print(create.add_to_bank("2005", "Q3?", ["a", "b", "c", "d"], ["a", "b"], 5))
    print("In question bank: ")
    for i in create.db.course_shelve['2005'].question_bank:
        print(i.question)
    print()

    print(create.modify_bank_questions("2005", "Q3?", "Q4?", ["a", "b", "c", "d"], ["a", "b"], 5))
    for i in create.db.course_shelve["2005"].question_bank:
        print(i.question)
    print()

    print("returns true if quiz modified")
    print(create.modify_quiz("2005", "quiz2", 1, date(2019,3,10), date(2019, 4, 20)))
    print()

    print("returns true if question in quiz was modified")
    print(create.modify_question("2005", "quiz1", "Q1?", "Q2?", ["a", "b", "c"], ["a"], 5))
    print()

    print("Returns true if quiz was copied to a new quiz name")
    print(create.copy_quiz("2005", "quiz2", "quiz3"))
    print()

    print("Before deletion")
    for i in create.db.course_shelve["2005"].quizzes:
        print(i)

    print("returns true if quiz was deleted: ")
    print(create.delete_quiz("2005", "quiz1"))
    print("After deletion: ")
    for i in create.db.course_shelve["2005"].quizzes:
        print(i)