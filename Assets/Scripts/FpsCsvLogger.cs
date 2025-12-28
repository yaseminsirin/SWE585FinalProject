using System.IO;
using UnityEngine;

public class FpsCsvLogger : MonoBehaviour
{
[Header("Logging")]
[Tooltip("Kaç saniyede bir FPS örneği alınsın")]
public float sampleInterval = 0.25f;

[Tooltip("Oluşturulacak CSV dosya adı")]
public string fileName = "fps_log.csv";

[Header("Experiment Parameters (Inspector'dan doldur)")]
public float amplitude;
public float frequency;
public float breakForce;
public int oliveCount;

private float timer = 0f;
private string filePath;

void Start()
{
// CSV'nin kaydedileceği TAM yol
filePath = Path.Combine(Application.persistentDataPath, fileName);

// Dosya yoksa başlık satırıyla oluştur
if (!File.Exists(filePath))
{
File.WriteAllText(
filePath,
"time_s,frame,fps,frame_ms,amplitude,frequency,breakForce,oliveCount\n"
);
}

// DOSYA YOLUNU CONSOLE'A YAZ (en kritik satır)
Debug.Log("FPS CSV PATH: " + filePath);
}

void Update()
{
timer += Time.unscaledDeltaTime;
if (timer < sampleInterval) return;
timer = 0f;

float delta = Mathf.Max(Time.unscaledDeltaTime, 0.000001f);
float fps = 1f / delta;
float frameMs = delta * 1000f;

string line =
$"{Time.realtimeSinceStartup:F3}," +
$"{Time.frameCount}," +
$"{fps:F2}," +
$"{frameMs:F2}," +
$"{amplitude:F3}," +
$"{frequency:F3}," +
$"{breakForce:F1}," +
$"{oliveCount}\n";

File.AppendAllText(filePath, line);
}
}