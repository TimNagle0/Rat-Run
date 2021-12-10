def create(db):
    class GameplayDataWheel(db.Model):
        __tablename__ = "GameplayWheel"
        gameLogID = db.Column(db.Integer, primary_key=True, autoincrement=True)
        participantID = db.Column(db.Integer, db.ForeignKey('participant.participantID'))
        TimeStamp = db.Column(db.String)
        Level = db.Column(db.String)
        DataType = db.Column(db.String)
        CurrentTarget = db.Column(db.Integer)
        RotationSpeed = db.Column(db.Integer)
        TargetColor1 = db.Column(db.String)
        TargetColor2 = db.Column(db.String)
        CurrentPlayerSpeed = db.Column(db.Float)
        CurrentPlayerColor = db.Column(db.String)
        NewPlayerColor = db.Column(db.String)
        


    class SummaryDataWheel(db.Model):
        __tablename__ = "SummaryWheel"
        gameLogID = db.Column(db.Integer, primary_key=True, autoincrement=True)
        participantID = db.Column(db.Integer, db.ForeignKey('participant.participantID'))
        StartTime = db.Column(db.String)
        EndTime = db.Column(db.String)
        Level = db.Column(db.String)
        TotalTime = db.Column(db.Integer)
        TotalScore = db.Column(db.Integer)
        TotalDistance = db.Column(db.Integer)
        TotalKeyPresses = db.Column(db.Integer)
        TotalDirectionChanges = db.Column(db.Integer)
        TotalColorChanges = db.Column(db.Integer)
        FalseColorChanges = db.Column(db.Integer)
        FalseKeyPresses = db.Column(db.Integer)

    return GameplayDataWheel, SummaryDataWheel
