using System;
using Gameplay.Cameras;
using Gameplay.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Words
{
    public class Cluster : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<Cluster> OnDragEnded;
        
        [SerializeField] private TMP_Text _letters;

        private IInputService _inputService;
        private Transform _moveParent;

        public int Length { get; private set; }

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Initialize(string letters, Transform moveParent)
        {
            if (string.IsNullOrEmpty(letters))
                throw new Exception("Letters can't be empty");
            
            _letters.SetText(letters);
            Length = letters.Length;
            _moveParent = moveParent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_moveParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 position = new Vector3(
                _inputService.GetTouchPosition().x, 
                _inputService.GetTouchPosition().y,
                0);
            
            transform.position = position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnded?.Invoke(this);
        }
    }
}