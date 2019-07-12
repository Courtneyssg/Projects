#cssg02@mun.ca - 200917250
from flask import Flask, request
from CreateQuiz import Create
from datetime import date
from Course import Course

app = Flask(__name__)

session = {
    "user": 'instructor'
}