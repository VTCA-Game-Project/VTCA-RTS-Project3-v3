using UnityEngine;

namespace InterfaceCollection
{
    // interface for properties
    interface IGameEntiy
    {
        Vector3 Position    { get; }
        Vector3 Heading     { get; }
        Vector3 Velocity    { get; }
    }
    interface ISelectable
    {
        void Select();
        void UnSelect();
        void Action();
    }
    // interface for behavior
    interface IAttackable
    {
        void Attack();
    }
    interface IDetectEnemy
    {
        void DetectEnemy();
    }
    interface IProduce
    {
        void Produce(System.Enum type);
    }
}

