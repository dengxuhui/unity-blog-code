using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 摄像机聚焦示例代码
/// </summary>
public class EX_CameraFocus : MonoBehaviour
{
    #region 与示例代码不大相关的初始化过程

    //摄像机
    [SerializeField] private Camera mainCamera;
    //聚焦立方体
    [SerializeField] private Button btnCube;
    //聚焦球体
    [SerializeField] private Button btnSphere;

    [SerializeField]
    private Transform cube;
    [SerializeField]
    private Transform sphere;
    private Plane _plane;
    void Start()
    {
        btnCube.onClick.AddListener(OnFocusCubeClick);
        btnSphere.onClick.AddListener(OnFocusSphereClick);
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void OnFocusCubeClick()
    {
        CameraFocusAt(cube);
    }

    private void OnFocusSphereClick()
    {
        CameraFocusAt(sphere);
    }
    
    #endregion

    private void CameraFocusAt(Transform target)
    {
        var cp = CalcScreenCenterPosOnPanel();
        var tp = target.position;
        //1.直接移动
        // mainCamera.transform.Translate(tp - cp,Space.World);
        //2.使用tween移动
        mainCamera.transform.DOMove(mainCamera.transform.position + (tp - cp), 0.5f);
    }


    /// <summary>
    /// 屏幕中心点到panel上的坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 CalcScreenCenterPosOnPanel()
    {
        var ray = mainCamera.ScreenPointToRay(new Vector3((float) Screen.width / 2, (float)Screen.height / 2, 0));
        if (_plane.Raycast(ray, out var distance))
        {
            return ray.GetPoint(distance);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
