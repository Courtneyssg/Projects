#cssg02@mun.ca - 200917250
class Quiz:

    def __init__(self, quiz_name, attempts, start_time, end_time):
        self.quiz_name = quiz_name
        self.attempts = attempts
        self.start_time = start_time
        self.end_time = end_time
        self.quiz_questions = []
        self.total_worth = 0

