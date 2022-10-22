using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Column
    {
        public int buttonFloor;
        CallButton callButton;
        public string callButtonID;
        Elevator elevator;
        public int elevatorID;
        public Elevator bestElevator;
        public int gap;
        public int ID;
        public string status;
        public int amountOfFloors;
        public int amountOfElevators;
        public string requestedDirection;
        public int requestedFloor;
        public int userPosition;
        public List<CallButton> callButtonsList;
        public List<Elevator> elevatorsList;
        public List<int> servedFloorsList;
        
        
        public Column(int _id, string _status, int _amountOfFLoors, int _amountOfElevators, List<int> _servedFloors, bool _isBasement)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfFloors = _amountOfFLoors;
            this.amountOfElevators = _amountOfElevators;
            this.elevatorsList= new List<Elevator>();
            this.callButtonsList= new List<CallButton>();
            this.servedFloorsList = _servedFloors;

            this.createElevators(_amountOfFLoors, _amountOfElevators);
            this.createCallButtons(_amountOfFLoors, _isBasement);
        }

        public void createCallButtons(int _amountOfFloors, bool _isBasement){
            if(_isBasement){
                buttonFloor = -1;
                for(int i = 0; i < _amountOfFloors; i++){
                    callButton = new CallButton(callButtonID, "OFF", buttonFloor, "Up");
                    callButtonsList.Add(callButton);
                    buttonFloor -= 1;
                    callButtonID += 1;
                }
            } else {
                buttonFloor = 1;
                for(int j = 0; j < _amountOfFloors; j++){
                    callButton = new CallButton(callButtonID, "OFF", buttonFloor, "Down");
                    callButtonsList.Add(callButton);
                    buttonFloor += 1;
                    callButtonID += 1; 
                }
            }
        }

        public void createElevators(int _amountOfFloors, int _amountOfElevators){
            for(int k = 0; k < _amountOfElevators; k++){
                elevator = new Elevator(elevatorID, "idle", _amountOfFloors, 1);
                elevatorsList.Add(elevator);
                elevatorID++;
            }
        }

        //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int requestedFloor, string requestedDirection)
        {
            elevator = findElevator(requestedFloor, requestedDirection);
            elevator.addNewRequest(requestedFloor);
            elevator.move();

            elevator.addNewRequest(1);
            elevator.move();
            return elevator;
        }

        public Elevator findElevator(int requestedFloor, string requestedDirection){
            int bestScore = 6;
            int referenceGap = 10000000;
            BestElevatorInformations bestElevatorInformations = new BestElevatorInformations();
            
            
            if(requestedFloor == 1){
                foreach(Elevator elevator in this.elevatorsList){
                    //The elevator is at the lobby and already has some requests.
                    if(elevator.currentFloor == 1 && elevator.status == "stopped"){
                        bestElevatorInformations = checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is at the lobby and has no requests
                    else if(elevator.currentFloor == 1 && elevator.status == "idle"){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is lower than me and is coming up. It means that I'm requesting an elevator to go to a basement, and the elevator is on it's way to me.
                    else if(elevator.currentFloor < 1 && elevator.direction == "up"){
                        bestElevatorInformations = checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is above me and is coming down. It means that I'm requesting an elevator to go to a floor, and the elevator is on it's way to me
                    else if(elevator.currentFloor > 1 && elevator.direction == "down"){
                        bestElevatorInformations = checkIfElevatorIsBetter(3, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is not at the first floor, but doesn't have any request
                    else if(elevator.status == "idle"){
                        bestElevatorInformations = checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is not available, but still could take the call if nothing better is found
                    else{
                        bestElevatorInformations = checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                   // bestElevator = bestElevatorInformations;
                    // bestElevatorInformations.Add(bestScore);
                    // bestElevatorInformations.Add(referenceGap);
                } 
            } else {
                foreach(Elevator elevator in this.elevatorsList){
                    //The elevator is at the same level as me, and is about to depart to the first floor
                    if(requestedFloor == elevator.currentFloor && elevator.status == "stopped" && requestedDirection == elevator.direction){
                        bestElevatorInformations = checkIfElevatorIsBetter(1, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is lower than me and is going up. I'm on a basement, and the elevator can pick me up on it's way
                    else if(requestedFloor > elevator.currentFloor && elevator.direction == "up" && requestedDirection == "up"){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is higher than me and is going down. I'm on a floor, and the elevator can pick me up on it's way
                    else if(requestedFloor < elevator.currentFloor && elevator.direction == "down" && requestedDirection == "down"){
                        bestElevatorInformations = checkIfElevatorIsBetter(2, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is idle and has no requests
                    else if(elevator.status == "idle"){
                        bestElevatorInformations = checkIfElevatorIsBetter(4, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                    //The elevator is not available, but still could take the call if nothing better is found
                    else{
                        bestElevatorInformations = checkIfElevatorIsBetter(5, elevator, bestScore, referenceGap, bestElevator, requestedFloor);
                    }
                  //  bestElevator = bestElevatorInformations.bestElevator;
                    // bestElevatorInformations.Add(bestScore);
                    // bestElevatorInformations.Add(referenceGap);
                }
            }
            return bestElevatorInformations.bestElevator;
        }

        public BestElevatorInformations checkIfElevatorIsBetter(int scoreToCheck, Elevator newElevator, int bestScore, int referenceGap, Elevator bestElevator, int floor){
           BestElevatorInformations bestElevatorInformations = new BestElevatorInformations();
            if(scoreToCheck < bestScore){
                bestElevatorInformations.bestScore = scoreToCheck;
                bestElevatorInformations.bestElevator = newElevator;
                bestElevatorInformations.referenceGap = Math.Abs(newElevator.currentFloor - floor);
            } else if(bestScore == scoreToCheck){
                gap = Math.Abs(newElevator.currentFloor - floor);
                if(referenceGap > gap){
                    bestElevatorInformations.bestElevator = newElevator;
                    bestElevatorInformations.referenceGap = gap;
                }
            }
           // bestElevatorInformations.Add(bestElevator, bestScore, referenceGap);
            return bestElevatorInformations;
        }
    }
}