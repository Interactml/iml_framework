﻿using UnityEngine;
using System;

namespace InteractML
{
    /// <summary>
    /// Configurable data vector for configurable serial connections 
    /// </summary>
    [Serializable]
    public class IMLArray : IMLBaseDataType
    {

        [SerializeField]
        private float[] m_Values;

        public override float[] Values { get { return m_Values; } set { if (value != null) SetValues(value); } }

        private IMLSpecifications.DataTypes m_DataType = IMLSpecifications.DataTypes.Array;

        public override IMLSpecifications.DataTypes DataType { get { return m_DataType; } }

        public IMLArray()
        {
            if (m_Values == null)
                m_Values = new float[0];

            m_DataType = IMLSpecifications.DataTypes.Array;
        }

        public IMLArray(int serialVectorSize)
        {
            m_DataType = IMLSpecifications.DataTypes.Array;
            m_Values = new float[serialVectorSize];
        }

        public IMLArray(IMLBaseDataType newData)
        {
            m_DataType = IMLSpecifications.DataTypes.Array;

            if (newData.Values != null && newData.Values.Length > 0)
                SetValues(newData.Values);
        }

        public IMLArray(float[] newData)
        {
            m_DataType = IMLSpecifications.DataTypes.Array;

            if (newData != null && newData.Length > 0)
                SetValues(newData);

        }

        public void SetValues(float[] newValues)
        {
            if (m_Values == null||m_Values.Length == 0||m_Values.Length != newValues.Length)
            {
                //debugging code needs revisting
                if(newValues == null)
                {
                    newValues = new float[0];
                }
                m_Values = new float[newValues.Length];
            }

            for (int i = 0; i < newValues.Length; i++)
            {
                m_Values[i] = newValues[i];
            }
      
        }

        public float[] GetValues()
        {
            return m_Values;
        }

    }

}
