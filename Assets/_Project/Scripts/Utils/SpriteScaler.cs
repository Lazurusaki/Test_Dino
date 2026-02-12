using UnityEngine;

namespace _Project.Scripts.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteScaler : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _scale = 1f;
        [SerializeField] private bool _fitInside = false;

        private void Start()
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr == null || sr.sprite == null) return;

            var camera = Camera.main;
            if (camera == null) return;

            float camHeight = camera.orthographicSize * 2f;
            float camWidth = camHeight * camera.aspect;

            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            float scaleX = camWidth / spriteWidth;
            float scaleY = camHeight / spriteHeight;

            float scale = _fitInside ? Mathf.Min(scaleX, scaleY) : Mathf.Max(scaleX, scaleY);

            transform.localScale = new Vector3(scale * _scale, scale * _scale, 1);

            transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z);
        }
    }
}