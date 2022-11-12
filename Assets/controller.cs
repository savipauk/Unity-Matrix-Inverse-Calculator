using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MathNet.Numerics.LinearAlgebra;
using System.IO;

public class controller : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_InputField matrixInputField;
    public TMP_InputField inverseMatrixInputField;
    public Toggle toggle;
    public Canvas canvas;
    public Button reset;
    public Button update;
    public Button export;
    public Vector2 matrixPosition = new Vector2(0, 0);
    public Vector2 invertedMatrixPosition = new Vector2(250, 0);
    public Vector2 gridCellSize = new Vector2(46, 30);
    public Vector2 gridSpacing = new Vector2(5, 5);
    public TMP_Text detLabel;
    public TMP_Text savedMessage;

    int matrixSize;
    double determinant;
    Matrix<double> matrix;
    Matrix<double> inverseMatrix;

    GameObject MatrixGrid;
    GameObject invertedMatrixGrid;

    void Start()
    {
        savedMessage.GetComponent<TMP_Text>().alpha = 0;
        savedMessage.GetComponent<TMP_Text>().text += Path.GetDirectoryName(Application.dataPath) + "\\matrix.txt";
    }

    public void ValueChanged()
    {
        string txt = inputField.text;
        if (txt.Length > 0 && (txt[0] == '-' || txt[0] == '0' || txt[0] == '1')) inputField.text = "2";
    }

    public void CreateMatrix()
    {
        if (inputField.text == "") return;

        reset.gameObject.SetActive(true);
        update.gameObject.SetActive(true);
        export.gameObject.SetActive(true);

        matrixSize = int.Parse(inputField.text);

        if (GameObject.Find("Grid") != null)
            Destroy(GameObject.Find("Grid"));

        MatrixGrid = new GameObject("Grid");

        MatrixGrid.AddComponent<RectTransform>();
        MatrixGrid.transform.SetParent(canvas.transform, true);
        MatrixGrid.transform.localScale = new Vector3(1, 1, 1);
        MatrixGrid.transform.localPosition = matrixPosition;

        MatrixGrid.AddComponent<GridLayoutGroup>();
        GridLayoutGroup grid = MatrixGrid.GetComponent<GridLayoutGroup>();
        RectTransform rt = MatrixGrid.GetComponent<RectTransform>();
        grid.cellSize = gridCellSize;
        grid.spacing = gridSpacing;
        rt.sizeDelta = new Vector2(matrixSize * grid.cellSize.x + matrixSize * grid.spacing.x, matrixSize * grid.cellSize.y + matrixSize * grid.spacing.y);
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;

        matrix = Matrix<double>.Build.Dense(matrixSize, matrixSize);

        int k = 0;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                TMP_InputField matrixElement = Instantiate(matrixInputField, grid.transform);
                matrixElement.name = "m" + k;
                double num = Random.Range(-100, 100);
                if (toggle.isOn)
                {
                    matrixElement.text = "" + num;
                    matrix[i, j] = num;
                }
                else
                {
                    matrixElement.text = "0";
                    matrix[i, j] = 0;
                }
                k++;
            }
        }

        inverseMatrix = matrix.Inverse();
        determinant = matrix.Determinant();
        detLabel.text = determinant.ToString();

        if (GameObject.Find("iGrid") != null)
            Destroy(GameObject.Find("iGrid"));

        invertedMatrixGrid = new GameObject("iGrid");

        invertedMatrixGrid.AddComponent<RectTransform>();
        invertedMatrixGrid.transform.SetParent(canvas.transform, true);
        invertedMatrixGrid.transform.localScale = new Vector3(1, 1, 1);
        invertedMatrixGrid.transform.localPosition = invertedMatrixPosition;

        invertedMatrixGrid.AddComponent<GridLayoutGroup>();
        GridLayoutGroup iGrid = invertedMatrixGrid.GetComponent<GridLayoutGroup>();
        RectTransform iRt = invertedMatrixGrid.GetComponent<RectTransform>();
        iGrid.cellSize = gridCellSize;
        iGrid.spacing = gridSpacing;
        iRt.sizeDelta = new Vector2(matrixSize * iGrid.cellSize.x + matrixSize * iGrid.spacing.x, matrixSize * iGrid.cellSize.y + matrixSize * iGrid.spacing.y);
        iGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;

        k = 0;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                TMP_InputField matrixElement = Instantiate(inverseMatrixInputField, iGrid.transform);
                matrixElement.name = "i" + k;
                matrixElement.text = "" + inverseMatrix[i, j];
                k++;
            }
        }
    }

    public void ResetMatrix()
    {
        reset.gameObject.SetActive(false);
        update.gameObject.SetActive(false);
        export.gameObject.SetActive(false);
        Destroy(MatrixGrid);
        Destroy(invertedMatrixGrid);
    }

    public void UpdateMatrix()
    {
        MatrixGrid = GameObject.Find("Grid");
        for (int i = 0; i < MatrixGrid.transform.childCount; i++)
            matrix[i / matrixSize, i % matrixSize] = double.Parse(GameObject.Find("m" + i).GetComponent<TMP_InputField>().text);

        inverseMatrix = matrix.Inverse();
        determinant = matrix.Determinant();
        detLabel.text = determinant.ToString();
      
        invertedMatrixGrid = GameObject.Find("iGrid");

        for(int i = 0; i < invertedMatrixGrid.transform.childCount; i++)
            GameObject.Find("i" + i).GetComponent<TMP_InputField>().text = "" + inverseMatrix[i / matrixSize, i % matrixSize];  
    }

    public void ExportMatrix()
    {
        savedMessage.GetComponent<TMP_Text>().alpha = 1;

        using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Application.dataPath) + "\\matrix.txt"))
        {
            sw.WriteLine(matrix.ToString());
            sw.WriteLine(inverseMatrix.ToString());
            sw.WriteLine("Determinant: " + determinant);
        }

        StartCoroutine("SavedPopup");
    }

    IEnumerator SavedPopup()
    {
        for(int i = 0; i < 6; i++)
        {
            if(i == 5)
            {
                for(int j = 9; j >= 0; j--)
                {
                    savedMessage.GetComponent<TMP_Text>().alpha = 0.1f * j;
                    yield return new WaitForSecondsRealtime(0.05f);
                }
            }
            yield return new WaitForSecondsRealtime(1f);
        }
        
    }
}