using System;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoInstaller {
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Player _player;
    [SerializeField] private Enemy _enemy;

    public override void InstallBindings() {
        RegisterInputService();
        RegisterPlayer();
        RegisterEnemy();
    }

    private void RegisterEnemy() {
        Container
            .BindInstance<Enemy>(_enemy)
            .AsSingle()
            .NonLazy();
    }

    private void RegisterPlayer() {
        Container
            .BindInstance<Player>(_player)
            .AsSingle()
            .NonLazy();
    }

    private void RegisterInputService() {
        IInputService instance = new JoystickInput(_joystick);
        Container
            .BindInstance<IInputService>(instance)
            .AsSingle()
            .NonLazy();
    }
}