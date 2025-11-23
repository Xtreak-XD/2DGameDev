public interface ILogoState
{
    void Enter(Logo logo);

    void Exit();
    
    void Update(double delta);
}