using System.Threading;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Elevator
    {
        public int destination;
        public int screenDisplay;
        public int ID;
        public string status;
        public int amountOfFloors;
        public int currentFloor;
        public string direction;
        public string overWeightAlarm;
       // bool obstruction = false;
       // bool overweight = false;
       // public Door door;
        public List<int> floorRequestsList;
        public List<int> requestList;
        public List<int> completedRequestsList;
        public int requestedFloor;
        public Elevator(int _id, string _status, int _amountOfFLoors, int _currentFloor)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfFloors = _amountOfFLoors;
            this.currentFloor = _currentFloor;
            Door door = new Door(_id, "closed");
            this.floorRequestsList= new List<int>();
            this.direction = null;
           // this.overweight = false;
        }
        public void move()
        {
            while(this.floorRequestsList.Count != 0){
                destination = (int) floorRequestsList[0];
                this.status = "moving";
                if(this.currentFloor < destination){
                    this.direction = "up";
                 //   this.sortFloorList();
                    while(this.currentFloor < destination){
                        currentFloor += 1;
                        this.screenDisplay = this.currentFloor;
                    }
                } else if(this.currentFloor > destination){
                    this.direction = "down";
                   // this.sortFloorList();
                    while(this.currentFloor > destination){
                        currentFloor -= 1;
                        this.screenDisplay = this.currentFloor;
                    }
                }
                this.status = "stopped";
               // this.operateDoors();
                floorRequestsList.RemoveAt(0);
              //  this.completedRequestsList.Add(destination);
            }
            this.status = "idle";
        }

        public void sortFloorList(){
            if(this.direction == "up"){
                this.floorRequestsList.Sort();
            } else {
                this.floorRequestsList.Reverse();
            }
        }

        // public void operateDoors(){
        //     this.door.status = "opened";
        //     //wait 5 seconds
        //     if(!overweight){ //this.what????
        //         this.door.status = "closing";
        //         if(!obstruction){ 
        //             this.door.status = "closed";
        //         } else {
        //             this.operateDoors();
        //         }
        //     } else {
        //         while(overweight){
        //             overWeightAlarm = "activated";
        //         }
        //         this.operateDoors();
        //     }
        // }

        public void addNewRequest(int requestedFloor){
            if(!this.floorRequestsList.Contains(requestedFloor)){
                floorRequestsList.Add(requestedFloor);
            }
            if(this.currentFloor < requestedFloor){
                this.direction = "up";
            } else if(this.currentFloor > requestedFloor){
                this.direction = "down";
            }
        }
    }
}