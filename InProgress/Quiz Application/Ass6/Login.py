"""Login functionality for online quiz service"""  #Chris Smith
from temp_persist import Persist

def authenticate(userID, password):
    """Validate user ID and password against database

        Raises:
          KeyError - if userID does not exist
          ValueError - bad password

        return: boolean indicating pass or fail
    """
    return True

def isInstructor(userID):
    """Return whether user is an instructor

        Raises:
          KeyError - if userID does not exist
          ValueError - userID is not a string

        return: boolean indicating user is or is not instructor
    """
    return True

def addAccount(userID, password, isInstructor=False):
    """Register a user to the database

        Raises:
          KeyError - if userID is already in use
          ValueError - bad parameter passing

        return: boolean indicating pass or fail
    """
    return True
