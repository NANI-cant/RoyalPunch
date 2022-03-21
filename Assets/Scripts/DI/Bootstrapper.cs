using Infrastructure;
using UI;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoInstaller {
    [Header("UI")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private StartPanel _startPanel;

    [Header("Gameplay")]
    [SerializeField] private Player _player;
    [SerializeField] private Enemy _enemy;

    [Header("Global")]
    [SerializeField] private Camera _mainCamera;

    public override void InstallBindings() {
        IInputService inputService = RegisterInstanceSingle<IInputService>(new JoystickInput(_joystick));

        RegisterInstanceSingle<Camera>(_mainCamera);
        RegisterInstanceSingle<Player>(_player);
        RegisterInstanceSingle<Enemy>(_enemy);
        RegisterInstanceSingle<Game>(new Game(_player, _enemy, _startPanel, inputService));
    }

    private T RegisterInstanceSingle<T>(T instance) {
        Container
            .BindInstance<T>(instance)
            .AsSingle()
            .NonLazy();
        return instance;
    }
}