using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakU.Fireables;

namespace DanmakU{



[AddComponentMenu("DanmakU/Danmaku Emitter")]
public class CustomBulletPattern : DanmakuBehaviour
{
public DanmakuPrefab DanmakuType;

  public Range Speed = 5f;
  public Range AngularSpeed;
  public float rotationSpeed;
  private Range rotation;
  public Color Color = Color.white;
  public Range FireRate = 5;
  public float FrameRate;
  public Arc Arc;
  public Ring ring;
  public Line Line;
  public EuleurSpiral euleurSpiral;

  float timer;
  DanmakuConfig config;
  IFireable fireable;

  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  void Start() {
    if (DanmakuType == null) {
      Debug.LogWarning($"Emitter doesn't have a valid DanmakuPrefab", this);
      return;
    }
    var set = CreateSet(DanmakuType);
    set.AddModifiers(GetComponents<IDanmakuModifier>());
    fireable = Arc.Of(ring).Of(set);
  }

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update() {
    rotation += rotationSpeed * Time.deltaTime;
    if (fireable == null) return;
    var deltaTime = Time.deltaTime;
    if (FrameRate > 0) {
      deltaTime = 1f / FrameRate;
    }
    timer -= deltaTime;
    if (timer < 0) {
      config = new DanmakuConfig {
        Position = transform.position,
        // Rotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad * rotation,
        Rotation = Mathf.Deg2Rad * rotation,

        Speed = Speed,
        AngularSpeed = AngularSpeed,
        Color = Color
      };
      fireable.Fire(config);
      timer = 1f / FireRate.GetValue();
    }
  }
}
}
