using UnityEngine;
using System;
using System.Collections;

public class CreateMaze : MonoBehaviour {

    public GameObject mazeRow;
    public GameObject wallLateral;
    public GameObject wallBottom;
    public GameObject token;

    Vector3 initialPoint = new Vector3(5, 0, -7);
    int[] prevRow = new int[8];
    float endOfRow = -7f;

    bool didSomething = false;

    // Update is called once per frame
    void Update()
    {

        if ((!didSomething) && transform.position.z < 2.0f)
        {
            prevRow = CreateFirstRow();
            didSomething = true;
        }

        if (transform.position.z < endOfRow + 0.3){
            //if player is approaching edge of row create a new row
            float oldEndOfRow = endOfRow;
            endOfRow = endOfRow - 7f;

            Vector3 newRowPos = new Vector3(5, 0, endOfRow);
            prevRow = CreateNewRow(prevRow, newRowPos, oldEndOfRow);
        }
    }

    //----------------------- Create the first maze row with tokens in each cell -------------------
    int[] CreateFirstRow()
    {
        System.Random rnd = new System.Random(); //used to make random configurations
        int[] array = new int[8]; //a maze row has 8 cells

        //STEP 1 : create the row 
        Instantiate(mazeRow, initialPoint, Quaternion.identity);



        // STEP 2 : put lateral walls 
        for (int i = 0; i < 8; i++){
            int putWall = rnd.Next(2);  //if putWall == 1 then put a wall
            array[i] = i; //map every cell to a single index (sets of 1)
            if (putWall == 1 && i !=0){
                float offset = 5f * (float)i;
                float x = 5.25f + offset;
                Instantiate(wallLateral, new Vector3(x, 2f, -3.25f), Quaternion.identity);
            }
            else{
                // if no walls separate cells, identify them as a single set

                if(i !=0){
                    array[i] = array[i - 1];
                }

            }
        } //finished step 2


        //STEP 3 : put bottom walls
        for (int j = 0; j < 8; j++){
            ArrayList setIndex = new ArrayList();
            //identify sets of same indexes 
            setIndex.Add(j);

            for (int s = j + 1; s < 8; s++){
                if(array[s] == array[j]){
                    setIndex.Add(s);
                }
                else{
                    //look for different set with x
                    j = s;
                    s = 8; //exit nested for loop
                }

                int sizeSet = setIndex.Count;

                int numOfWalls = rnd.Next(sizeSet);

                if(sizeSet > 1){
                   
                    //add bottom walls only when sets are bigger than 1
                    while (numOfWalls != 0){
                        numOfWalls--;
                        int indexAsInt = (int)setIndex[0];
                        float index = (float)indexAsInt;
                        setIndex.RemoveAt(0);

                        //add bottom wall at the corresponding cell
                        float x = 7.5f + (5 * index);
                        Instantiate(wallBottom, new Vector3(x, 2f, -6.75f), Quaternion.identity);
                        //lastly, update the old index to an empty one
                        // to do this, we set a negative index of -1
                        //so that we can update with a new id at the next iteration if needed
                        array[indexAsInt] = -1;
                    }
                }
            }
        } //finish step 3


        //STEP 4 : spawn tokens

        //spawn coins in each cell
        int actualNumofTokens = 8;

        //note: coins will appear at the middle of maze cells
        int indexCounter = 0;
        while (actualNumofTokens != 0)
        {

            actualNumofTokens--;
            float offset = 5f * indexCounter;
            float x = 7.5f + offset;
            Instantiate(token, new Vector3(x, 0.6f, (endOfRow / 2)), Quaternion.identity);

            indexCounter++;
        }

        return array;

    }




    // ---------------------- Create a new row to the endless maze --------------------

    int[] CreateNewRow(int[] oldRow, Vector3 rowPosition, float zPos){
        System.Random rnd = new System.Random();


        Debug.Log("new array before adding walls");

        foreach (var item in oldRow)
        {
            Debug.Log(item);
        }

        //STEP 1: Create the row terrain
        Instantiate(mazeRow, rowPosition, Quaternion.identity);

        int lastID = oldRow[7];
        //generate new unused integers to identify new sets
        int newID = lastID + 8;

        for (int a = 0; a < 8; a++)
        {
            if (oldRow[a] == -1)
            {
                oldRow[a] = newID;
                newID++;
            }
        }


        //STEP 2: add lateral walls
        for (int i = 0; i < oldRow.Length - 1; i++) {

            if (oldRow[i] == oldRow[i + 1])
            {
                float factor = (float)(i + 1);
                float x = 5.25f + (5f * factor);
                Instantiate(wallLateral, new Vector3(x, 2f, -3.25f + zPos), Quaternion.identity);
                oldRow[i + 1] = newID;
                newID++;
            }
            else
            {
                int putWall = rnd.Next(2);
                if (putWall == 1 && (i != 0))
                {
                   
                    float factor = (float)(i + 1);
                    float x = 5.25f + (5f * factor);
                    Instantiate(wallLateral, new Vector3(x, 2f, -3.25f + zPos), Quaternion.identity);

                }
                else
                {
                    if (i != 0)
                    {
                        oldRow[i+1] = oldRow[i ];
                    }
                }

             }
          }//end adding lateral walls


        //Step 3: add bottom walls
        for (int j = 0; j < 8; j++)
        {
            ArrayList set = new ArrayList();
            //identify the sets
            set.Add(j);
            for (int s = j + 1; s < 8; s++)
            {
                if (oldRow[s] == oldRow[j])
                {
                    set.Add(s);
                }
                else
                {
                    //no other unions found, look at the set
                    j = s;
                    s = 8; //stop iterating inside the nested for loop
                }

                int sizeSet = set.Count;
                if (sizeSet > 1)
                {

                    int numOfWalls = rnd.Next(sizeSet);
                    while (numOfWalls != 0)
                    {
                        numOfWalls = numOfWalls - 1;
                        int indexAsInt = (int)set[0];
                        float index = (float)indexAsInt;
                        set.RemoveAt(0);
                        //Add Bottom wall at index
                        float x = 7.5f + (5 * index);
                        Instantiate(wallBottom, new Vector3(x, 2f, -6.75f + zPos), Quaternion.identity);
                        //lastly, update the old index to an empty one
                        // to do this, we set a negative index of -1
                        //so that we can update with a new id at the next iteration if needed
                        oldRow[indexAsInt] = -1;

                    }
                }

            }
        } //stop adding botton walls





        return oldRow;
    }
}
