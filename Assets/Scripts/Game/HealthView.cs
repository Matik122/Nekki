using Core;
using Support;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthView : DisposableBehaviour<HealthView.Model>
    {
        public class Model
        {
            public float Health;
            public ReactiveProperty<float> OnHealthChange;
            
            public Model(float health, ReactiveProperty<float> onHealthChange)
            {
                Health = health;
                OnHealthChange = onHealthChange;
            }
        }

        [SerializeField] private Image _healthBar;
        
        protected override void OnInit()
        {
            base.OnInit();

            ActiveModel.OnHealthChange
                .SafeSubscribe(UpdateHealthBar)
                .AddTo(Disposables);

            _healthBar.fillAmount = ActiveModel.Health;
        }
        
        private void UpdateHealthBar(float health)
        {
            _healthBar.fillAmount = health / ActiveModel.Health;
        }
    }
}