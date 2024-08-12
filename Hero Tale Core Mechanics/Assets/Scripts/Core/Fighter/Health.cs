namespace Core
{
    public class Health
    {
        private float _currentHealth;
        private readonly FighterData _data;

        public Health(FighterData data)
        {
            _data = data;
            _currentHealth = _data.Health;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0) _currentHealth = 0;
        }

        public bool IsAlive()
        {
            return _currentHealth > 0;
        }

        public float GetHealthPercentage()
        {
            return _currentHealth / _data.Health;
        }
    }
}