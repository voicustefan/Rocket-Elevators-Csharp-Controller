using System;
using System.Collections.Generic;


namespace Commercial_Controller
{
    public class Battery
    {
        public int _amountOfBasements;
        public int _amountOfColumns;
        public int _amountOfFloors;
        public int amountOfFloorsPerColumn;
        public int buttonFloor;
        public int columnID;
        public int _floor;
        public string floorRequestButtonID;
        public int ID;
        public string requestedDirection;
        public string status;
        public List<Column> columnsList;
        public List<FloorRequestButton> floorButtonsList;
        public List<FloorRequestButton> floorRequestButtonsList;
        public List<int> servedFloors = new List<int>();
        public Column column;
        public Elevator elevator;
       // public int column;
        public Battery(int _id, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _id;
            this.status = "online";
            this.columnsList = new List<Column>();
            this.floorRequestButtonsList = new List<FloorRequestButton>();
            this.floorButtonsList = new List<FloorRequestButton>();
            

            if(_amountOfBasements > 0){
                this.createBasementFloorRequestButtons(_amountOfBasements);
                this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
                _amountOfColumns -= 1;
            }

            this.createFloorRequestButtons(_amountOfFloors);
            this.createColumns(_amountOfColumns, _amountOfFloors, _amountOfBasements, _amountOfElevatorPerColumn);
        }

        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn){
            List<int> servedFloors = new List<int>();
            _floor = -1;
            for(int i = 0; i < _amountOfBasements; i++){
                servedFloors.Add(_floor);
                _floor -= 1;
            }

            Column column = new Column(columnID, "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloors, true);
            this.columnsList.Add(column);
            columnID++;
        }

        public void createColumns(int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn){
            amountOfFloorsPerColumn = (_amountOfFloors/_amountOfColumns);
            _floor = 1;
            for(int j = 0; j < _amountOfColumns; j++){
                List<int> servedFloors = new List<int>();
                for(int k = 0; k < amountOfFloorsPerColumn; k++){
                    if(_floor <= _amountOfFloors){
                        servedFloors.Add(_floor);
                        _floor += 1;
                    }
                }
                Column column = new Column(columnID, "online", _amountOfFloors, _amountOfElevatorPerColumn, servedFloors, false);
                columnsList.Add(column);
                columnID += 1;
            }
        }

        public void createFloorRequestButtons(int _amountOfFLoors){
            buttonFloor = 1;
            for(int k = 0; k < _amountOfFloors; k++){
                FloorRequestButton floorRequestButton = new FloorRequestButton((k+1).ToString(), "OFF", buttonFloor, "Up");
                floorButtonsList.Add(floorRequestButton);
                buttonFloor += 1;
                floorRequestButtonID += 1;
            }
        }

        public void createBasementFloorRequestButtons(int _amountOfBasements){
            buttonFloor = -1;
            for(int l = 0; l < _amountOfBasements; l++){
                FloorRequestButton floorRequestButton = new FloorRequestButton((l+1).ToString(), "OFF", buttonFloor, "Down");
                floorButtonsList.Add(floorRequestButton);
                buttonFloor -= 1;
                floorRequestButtonID += 1;
            }
        }

        public Column findBestColumn(int _requestedFloor)
        {
            foreach(Column column in this.columnsList){
                if(column.servedFloorsList.Contains(_requestedFloor)){
                    return column;
                }
            }
            
            return column;
        }
        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            column = this.findBestColumn(_requestedFloor);
            elevator = column.findElevator(1, _direction);
            elevator.addNewRequest(1);
            elevator.move();
            elevator.addNewRequest(_requestedFloor);
            elevator.move();

            return (column, elevator);
        }
    }
}

