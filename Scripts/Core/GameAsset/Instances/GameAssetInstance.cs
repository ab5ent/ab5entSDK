using System;
using ab5entSDK.Core;
using UnityEngine;

[Serializable]
public class GameAssetInstance
{
    [field: SerializeField] private string _definitionId;

    [field: SerializeField] private Guid _instanceId;

    [field: SerializeField] private int _quantity;

    public string DefinitionId => _definitionId;

    public Guid InstanceId => _instanceId;

    public int Quantity => _quantity;

    public GameAssetInstance(string definitionId, int quantity, Guid instanceId = default(Guid))
    {
        _definitionId = definitionId;
        _instanceId = instanceId;
        _quantity = quantity;
    }

    public GameAssetInstance(GameAssetDefinition definition, bool defaultInstance = true)
    {
        _definitionId = definition.Id;
        _instanceId = defaultInstance ? Guid.Empty : Guid.NewGuid();
        _quantity = 0;
    }

    #region Methods

    public override int GetHashCode()
    {
        return HashCode.Combine(_instanceId, _definitionId);
    }

    public override bool Equals(object obj)
    {
        if (obj is not GameAssetInstance other)
        {
            return false;
        }

        return _instanceId == other._instanceId && _definitionId == other._definitionId;
    }

    public void SetQuantity(int newQuantity)
    {
        _quantity = newQuantity;
    }

    public void ChangeQuantity(int modifyQuantity)
    {
        _quantity = Mathf.Max(_quantity + modifyQuantity, 0);
    }

    #endregion

}