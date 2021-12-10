import datetime
from flask import Blueprint, render_template
from BOFS.util import *
from BOFS.globals import db
from BOFS.admin.util import verify_admin

# The name of this variable must match the folder's name.
Rat_Run_Wheel = Blueprint('Rat_Run_Wheel', __name__,
                          static_url_path='/Rat_Run_Wheel',
                          template_folder='templates',
                          static_folder='static')


def handle_game_post():
    if(request.form['formType'] == 'Continuous'):

        log = db.GameplayDataWheel()
        log.participantID = session['participantID']
        log.TimeStamp = request.form['timeStamp']
        log.Level = request.form['level']
        log.DataType = request.form['dataType']
        log.CurrentTarget = request.form['currentTarget']
        log.RotationSpeed = request.form['rotationSpeed']
        log.TargetColor1 = request.form['targetColor1']
        log.TargetColor2 = request.form['targetColor2']
        log.CurrentPlayerSpeed = request.form['currentSpeed']
        log.CurrentPlayerColor = request.form['currentColor']
        log.NewPlayerColor = request.form['newColor']
        db.session.add(log)
        db.session.commit()
        return ""
    else:
        log = db.SummaryDataWheel()
        log.participantID = session['participantID']
        log.StartTime = request.form['startTime']
        log.EndTime = request.form['endTime']
        log.Level = request.form['level']
        log.TotalTime = request.form['totalTime']
        log.TotalScore = request.form['totalScore']
        log.TotalDistance = request.form['totalDistance']
        log.TotalKeyPresses = request.form['totalKeyPresses']
        log.TotalDirectionChanges = request.form['totalDirectionChanges']
        log.TotalColorChanges = request.form['totalColorChanges']
        log.FalseColorChanges = request.form['falseColorChanges']
        log.FalseKeyPresses = request.form['falseKeyPresses']
        
        db.session.add(log)
        db.session.commit()
        return ""


@Rat_Run_Wheel.route("/Rat_Run_Wheel", methods=['POST', 'GET'])
@verify_correct_page
@verify_session_valid
def game_custom():
    if request.method == 'POST':
        return handle_game_post()
    return render_template("Rat_Run_Wheel.html")


@Rat_Run_Wheel.route("/fetch_condition")
@verify_session_valid
def fetch_condition():
    return str(session['condition'])


