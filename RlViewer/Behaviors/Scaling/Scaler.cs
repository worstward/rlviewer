﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Scaling
{
    public class Scaler
    {
        public Scaler(float scaleFactor = 1)
        {
            ScaleFactor = scaleFactor;
        }

        private float _scaleFactor;
        public float ScaleFactor
        {
            get
            {
                return _scaleFactor; 
            }
            private set 
            {
                var factor = value <= _maxZoom ? value : _maxZoom;
                factor = factor > _minZoom ? factor : _minZoom;
                _scaleFactor = factor;
            }
        }

        private const float _minZoom = 0.015625f;
        public float MinZoom
        {
            get { return _minZoom; }
        }

        private const float _maxZoom = 32f;
        public float MaxZoom
        {
            get { return _maxZoom; }
        }
    }
}
