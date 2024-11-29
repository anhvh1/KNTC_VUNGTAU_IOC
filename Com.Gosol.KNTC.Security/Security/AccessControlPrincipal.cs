namespace Com.Gosol.KNTC.Security
{
    using System;
    using System.Collections;
    using System.Security.Claims;
    using System.Security.Principal;

    public class AKNTCessControlPrincipal : IPrincipal
    {
        private Hashtable _aKNTCessGroups;
        private Hashtable _acl;
        private Hashtable _aclType;
        private AKNTCessControlIdentity _userIdentity;

        private AKNTCessControlPrincipal()
        {
            throw new AKNTCessControlExceptions("CanNotCreateClass");
        }

        private AKNTCessControlPrincipal(Hashtable aKNTCessGroups, Hashtable acl, Hashtable aclType, Hashtable userInfo)
        {
            this._aKNTCessGroups = aKNTCessGroups;
            this._acl = acl;
            this._aclType = aclType;
            this._userIdentity = AKNTCessControlIdentity.CreateInstance(userInfo);
            
        }

        public static AKNTCessControlPrincipal CreateInstance(Hashtable aKNTCessGroups, Hashtable acl, Hashtable aclType, Hashtable userInfo)
        {
            return new AKNTCessControlPrincipal(aKNTCessGroups, acl, aclType, userInfo);
        }

        public bool HasPermission(EntityType entityType, AccessLevel aKNTCessLevel)
        {
            string key = Convert.ToInt32(entityType).ToString();
            if (this._aclType.ContainsKey(key))
            {
                int num = Convert.ToInt32(aKNTCessLevel);
                return (num == (num & ((int) this._aclType[key])));
            }
            return (aKNTCessLevel == AccessLevel.NoAKNTCess);
        }

        public bool HasPermission(object entityType, AccessLevel aKNTCessLevel)
        {
            if (entityType.ToString().Equals("?"))
            {
                return false;
            }
            if (entityType.ToString().Equals("*"))
            {
                return true;
            }
            string key = Convert.ToInt32(entityType).ToString();
            if (this._aclType.ContainsKey(key))
            {
                int num = Convert.ToInt32(aKNTCessLevel);
                return (num == (num & ((int) this._aclType[key])));
            }
            return (aKNTCessLevel == AccessLevel.NoAKNTCess);
        }

        public bool HasPermission(object entityType, string aKNTCessLevel)
        {
            if (entityType.ToString().Equals("?") || aKNTCessLevel.Equals("?"))
            {
                return false;
            }
            if (entityType.ToString().Equals("*") || aKNTCessLevel.Equals("*"))
            {
                return true;
            }
            bool flag = false;
            if (Enum.IsDefined(typeof(AccessLevel), aKNTCessLevel))
            {
                AccessLevel level = (AccessLevel) Enum.Parse(typeof(AccessLevel), aKNTCessLevel);
                //flag = AKNTCessControl.User.HasPermission(entityType, level);
            }
            return true;
        }

        public bool HasPermission(object entityType, int entityID, AccessLevel aKNTCessLevel)
        {
            if (entityType.ToString().Equals("?"))
            {
                return false;
            }
            if (entityType.ToString().Equals("*"))
            {
                return true;
            }
            string key = Convert.ToInt32(entityType).ToString() + "$" + entityID.ToString();
            if (this._acl.ContainsKey(key))
            {
                int num = Convert.ToInt32(aKNTCessLevel);
                return (num == (num & ((int) this._acl[key])));
            }
            return (aKNTCessLevel == AccessLevel.NoAKNTCess);
        }

        public bool IsInRole(int RoleID)
        {
            return this._aKNTCessGroups.ContainsKey(RoleID);
        }
        public string NameGroup()
        {
            return this._aKNTCessGroups["TenNhom"].ToString();
        }
        
        public bool IsInRole(string roleName)
        {
            return this._aKNTCessGroups.ContainsValue(roleName);
        }

        public IIdentity Identity
        {
            get
            {
                return this._userIdentity;
            }
        }
    }
}
