public abstract class State
{
    protected const float crossFadeDuration = 0.1f;

    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();
}
