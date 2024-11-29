namespace Com.Gosol.KNTC.Security
{
    using System;

    [Serializable]
    internal class AKNTCessControlEntry
    {
        private int _aKNTCessRight;
        private ACLType _aclType;
        private int _entityid;
        private Com.Gosol.KNTC.Security.EntityType _entityType;

        public AKNTCessControlEntry(Com.Gosol.KNTC.Security.EntityType entityType, int entityid, int aKNTCessRight) : this(entityType, entityid, aKNTCessRight, ACLType.ObjectInstance)
        {
        }

        public AKNTCessControlEntry(Com.Gosol.KNTC.Security.EntityType entityType, int entityid, int aKNTCessRight, ACLType aclType)
        {
            this._entityType = entityType;
            if (aclType == ACLType.ObjectClass)
            {
                this._entityid = -1;
            }
            else if (aclType == ACLType.ObjectInstance)
            {
                this._entityid = entityid;
            }
            this._aclType = aclType;
            this._aKNTCessRight = aKNTCessRight;
        }

        public int AKNTCessRight
        {
            get
            {
                return this._aKNTCessRight;
            }
        }

        public ACLType ACEType
        {
            get
            {
                return this._aclType;
            }
        }

        public int Entityid
        {
            get
            {
                return this._entityid;
            }
        }

        public Com.Gosol.KNTC.Security.EntityType EntityType
        {
            get
            {
                return this._entityType;
            }
        }
    }
}
