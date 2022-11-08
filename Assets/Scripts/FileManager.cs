using System;
using UnityEngine;
using System.IO;


public class FileManager : MonoBehaviour
{
    // YOU HAVE THE CHANGE THE NUMBER 23 HERE ACCORDING TO NUMBER OF POINTS
    int NUM_OF_POINTS = 23;
    int[,] coordinates1 = new int[23, 3];
    int[,] coordinates2 = new int[23, 3]; 

    string[] coordinatesArray;
    string myFilePath, fileName;
    double[,] eqValues = new double[1,1];
    double[,] maxEqValues = new double[1,1];
    int max = 0;
    int ctr = 0;

    void Start()
    {
        fileName = "file1.txt";
        myFilePath = Application.dataPath + "/" + fileName;
        ReadFromTheFile(1);

        fileName = "file2.txt";
        myFilePath = Application.dataPath + "/" + fileName;
        ReadFromTheFile(2);

        selectPoints1();


        print("\n\n\n");
        print("MAX -> " + max);
        print("r1 -> " + maxEqValues[0, 0]);
        print("r2-> " + maxEqValues[0, 1]);
        print("r3-> " + maxEqValues[0, 2]);
        print("t1-> " + maxEqValues[1, 0]);
        print("t2-> " + maxEqValues[1, 1]);
        print("t3-> " + maxEqValues[1, 2]);

        drawTransformedObjects();
    }


    /*
     *  Reads files and parse coordinates
     *  Then draw spheres according to that data 
     *  Points of source file(first) are black; points of target file(second) are white
     */
    public void ReadFromTheFile(int index)
    {
        coordinatesArray = File.ReadAllLines(myFilePath);
        int i = 0;
        foreach (string line in coordinatesArray)
        {
            string[] parsedStr = line.Split(" ");
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(float.Parse(parsedStr[0]), float.Parse(parsedStr[1]), float.Parse(parsedStr[2]));

            if (index == 1)
            {
                sphere.GetComponent<Renderer>().material.color = Color.black;
                coordinates1[i, 0] = int.Parse(parsedStr[0]);
                coordinates1[i, 1] = int.Parse(parsedStr[1]);
                coordinates1[i, 2] = int.Parse(parsedStr[2]);

            }
            else
            {
                sphere.GetComponent<Renderer>().material.color = Color.white;
                coordinates2[i, 0] = int.Parse(parsedStr[0]);
                coordinates2[i, 1] = int.Parse(parsedStr[1]);
                coordinates2[i, 2] = int.Parse(parsedStr[2]);
            }
            i++;
        }
    }


    /*
     *  Select 3 points from firstFile(source file)
     */
    void selectPoints1()
    {
        for (int i = 0; i < NUM_OF_POINTS; i++)
        {
            int j = i + 1;
            ctr = 0;
            for (int k = j + 1; k < NUM_OF_POINTS; k++)
            {
                int currentCtr = selectPoints2(i, j, k);
                if ( currentCtr > max)
                {
                    max = currentCtr;
                    maxEqValues = eqValues;
                }
            } 
        }
    }

    /*
     *  Select 3 points from second file(target file)
     */
    int selectPoints2(int point1, int point2, int point3)
    {
        for (int i = 0; i < NUM_OF_POINTS; i++)
        {
            int j = i + 1;
            for (int k = j + 1; k < NUM_OF_POINTS; k++)
            {
                // if equation true for selected points, increase counter by 1. otherwise select 3 different points
                if(solveEquation(point1, point2, point3, i, j, k))
                    return ctr++;
                                   
            }
        }

        return ctr;
    }

    /*
     * Solves the equation and finds R and T values
     */
    Boolean solveEquation(int point1, int point2, int point3, int point4, int point5, int point6)
    {

        int point1_x = coordinates1[point1, 0];
        int point1_y = coordinates1[point1, 1];
        int point1_z = coordinates1[point1, 2];

       // print("point1_x-> " + point1_x + "   point1_y-> " + point1_y + "   point1_z-> " + point1_z);

        int point2_x = coordinates1[point2, 0];
        int point2_y = coordinates1[point2, 1];
        int point2_z = coordinates1[point2, 2];

       // print("point2_x-> " + point2_x + "   point2_y-> " + point2_y + "   point2_z-> " + point2_z);

        int point3_x = coordinates1[point3, 0];
        int point3_y = coordinates1[point3, 1];
        int point3_z = coordinates1[point3, 2];

       // print("point3_x-> " + point3_x + "   point3_y-> " + point3_y + "   point3_z-> " + point3_z);

        int point4_x = coordinates2[point4, 0];
        int point4_y = coordinates2[point4, 1];
        int point4_z = coordinates2[point4, 2];

       // print("point4_x-> " + point4_x + "   point4_y-> " + point4_y + "   point4_z-> " + point4_z);

        int point5_x = coordinates2[point5, 0];
        int point5_y = coordinates2[point5, 1];
        int point5_z = coordinates2[point5, 2];

       // print("point5_x-> " + point5_x + "   point5_y-> " + point5_y + "   point5_z-> " + point5_z);

        int point6_x = coordinates2[point6, 0];
        int point6_y = coordinates2[point6, 1];
        int point6_z = coordinates2[point6, 2];
        
       // print("point6_x-> " + point6_x + "   point6_y-> " + point6_y + "   point6_z-> " + point6_z);

        double r_0 = 0, r_1 = 0, r_2 = 0, t_0=0, t_1=0, t_2=0;

        if ((point1_x - point2_x) == 0 || (point1_y - point2_y) == 0 || (point1_z - point2_z) == 0)
            return false;

        r_0 = (double)(point4_x - point5_x) / (double)(point1_x - point2_x);
        r_1 = (double)(point4_y - point5_y) / (double)(point1_y - point2_y);
        r_2 = (double)(point4_z - point5_z) / (double)(point1_z - point2_z);

        t_0 = (point4_x) - (point1_x * r_0);
        t_1 = (point4_y) - (point1_y * r_1);
        t_2 = (point4_z) - (point1_z * r_2);

        if( (point3_x*r_0 + t_0 == point6_x) && (point3_y * r_1 + t_1 == point6_y) && (point3_z * r_2 + t_2 == point6_z))
        {
            eqValues = new double[,] { { r_0, r_1, r_2 }, { t_0, t_1, t_2 } };
            return true;
        }

        return false;
    }


    /*
     *  After finding best equation draws transformed spheres (red). 
     *  Draws a line between source(first) point and transformed one
     */
    void drawTransformedObjects()
    {
        for (int i = 0; i < NUM_OF_POINTS; i++)
        {
            // (x*r_0) + t_0 => new x
            // (y*r_1) + t_1 => new y
            // (z*r_2) + t_2 => new z

            double new_x = (coordinates1[i, 0] * maxEqValues[0, 0]) + maxEqValues[1, 0];
            double new_y = (coordinates1[i, 1] * maxEqValues[0, 1]) + maxEqValues[1, 1];
            double new_z = (coordinates1[i, 2] * maxEqValues[0, 2]) + maxEqValues[1, 2];


            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3((float)new_x, (float)new_y, (float)new_z);
            sphere.GetComponent<Renderer>().material.color = Color.red;

            Debug.DrawLine(new Vector3(coordinates1[i, 0], coordinates1[i, 1], coordinates1[i, 2]),
                           new Vector3((float)new_x, (float)new_y, (float)new_z),
                           Color.red, 1000000f, false);
        }
    }

}

