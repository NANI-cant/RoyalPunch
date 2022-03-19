using System;
using Infrastructure;
using UI;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoInstaller {
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Player _player;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private StartPanel _startPanel;
    [SerializeField] private Camera _mainCamera;

    public override void InstallBindings() {
        IInputService inputService = RegisterInputService();

        RegisterInstance<Game>(new Game(_startPanel, inputService));
        RegisterInstance<Camera>(_mainCamera);
        RegisterInstance<Player>(_player);
        RegisterInstance<Enemy>(_enemy);
    }

    private void RegisterInstance<T>(T instance) {
        Container
            .BindInstance<T>(instance)
            .AsSingle()
            .NonLazy();
    }

    private IInputService RegisterInputService() {
        IInputService instance = new JoystickInput(_joystick);
        Container
            .BindInstance<IInputService>(instance)
            .AsSingle()
            .NonLazy();
        return instance;
    }
}