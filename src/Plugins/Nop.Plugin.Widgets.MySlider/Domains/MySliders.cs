﻿
using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.MySlider.Domains
{
    public class MySliders : BaseEntity, IStoreMappingSupported
    {
        public string Name { get; set; }

        public int WidgetZoneId { get; set; }

        public int BackGroundPictureId { get; set; }

        public int DisplayOrder { get; set; }

        public bool Loop { get; set; }

        public int Margin { get; set; }

        public bool Nav { get; set; }

        public bool AutoPlay { get; set; }

        public int AutoPlayTimeout { get; set; }

        public bool AutoPlayHoverPause { get; set; }

        public int StartPosition { get; set; }

        public bool LazyLoad { get; set; }

        public int LazyLoadEager { get; set; }

        public string AnimateOut { get; set; }

        public string AnimateIn { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public bool LimitedToStores { get; set; }

    }
}
