using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Visualiser : MonoBehaviour
{
  [SerializeField] public GameObject cubePrefab;

  Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();
  // Start is called before the first frame update
  void Start()
  {
    Application.targetFrameRate = 30;
    var numSpirals = 8;
    var numCubes = 30;

    var cubeList2D = Enumerable.Range(1, numSpirals).Select((j, jIndex) =>
    {

      return Enumerable.Range(1, numCubes).Select((i) =>
      {
        // Create Cube
        var cube = Instantiate(cubePrefab);
        cube.transform.SetParent(transform);
        cube.name = $"{j}:{i}";
        return cube;
      }).Select((cube, index) =>
      {
        // Place cube in spiral
        var t = DegreeToRadian(360f / numCubes * index);
        var x = t * Mathf.Cos(t);
        var z = t * Mathf.Sin(t);
        cube.transform.position = Vector3.zero;
        var rotate = 360f / numSpirals * jIndex;
        cube.transform.Rotate(new Vector3(0, rotate, 0));
        cube.transform.Translate(new Vector3(x * 30f, 0, z * 30f), Space.Self);
        return cube;
      }).Select((cube, index) =>
      {
        // Scale cube
        var scaleFactor = 90f / numCubes * Mathf.Log(index + 1, 2);
        cube.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        cube.SetActive(true);
        return cube;
      }).Select(cube =>
      {
        // set cube color
        var mat = cube.GetComponent<MeshRenderer>().material;
        var color = Color.HSVToRGB(1f / numSpirals * jIndex, 1, 1);
        mat.color = color;
        return cube;
      });

    });

    // Using Selects and then reguritating them into a dictionary like this is almost definitely
    // not best practice. In my defence this is art and I can do whatever I want
    foreach (var cubeList in cubeList2D)
    {
      foreach (var cube in cubeList)
      {
        cubes.Add(cube.name, cube);
      };
    }
  }

  // Update is called once per frame
  void Update()
  {
    foreach (var cube in cubes.Values)
    {
      cube.transform.Rotate(new Vector3(0, 2f, 0));
      cube.transform.Translate(new Vector3(0.2f, 0, 0), Space.Self);
    }
  }

  private float DegreeToRadian(float angle)
  {
    return Mathf.PI * angle / 180f;
  }
}
