﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FriendHashMap : ValueChangeCallBack
{

    private Dictionary<uint, HashSet<Friend>> friendDict = new Dictionary<uint, HashSet<Friend>>();

    public List<Friend> getFriends(uint userId)
    {
        List<Friend> ret = new List<Friend>();
        {
            HashSet<Friend> friendSet = null;
            if (friendDict.TryGetValue(userId, out friendSet))
            {
                List<Friend> notCorrectFriends = new List<Friend>();
                // check and add
                {
                    foreach (Friend friend in friendSet)
                    {
                        if (friend.getDataParent() != null)
                        {
                            if (friend.user1Id.v == userId || friend.user2Id.v == userId)
                            {
                                ret.Add(friend);
                            }
                            else
                            {
                                Debug.LogError("not correct friend: " + friend + "; " + this);
                                notCorrectFriends.Add(friend);
                            }
                        }
                        else
                        {
                            Debug.LogError("friend data parent null: " + this);
                            notCorrectFriends.Add(friend);
                        }
                    }
                }
                // remove not correct from set
                if (notCorrectFriends.Count > 0)
                {
                    Predicate<Friend> checkRemove = delegate (Friend friend)
                    {
                        return notCorrectFriends.Contains(friend);
                    };
                    friendSet.RemoveWhere(checkRemove);
                }
            }
            else
            {
                Debug.LogError("friendDict null: " + userId + "; " + this);
            }
        }
        return ret;
    }

    private void addFriend(Friend friend)
    {
        // user1
        {
            uint user1Id = friend.user1Id.v;
            // Find friendSet
            HashSet<Friend> friend1Set = null;
            {
                if (!friendDict.TryGetValue(user1Id, out friend1Set))
                {
                    friend1Set = new HashSet<Friend>();
                    friendDict.Add(user1Id, friend1Set);
                }
            }
            // Add
            friend1Set.Add(friend);
            // delegate
            {
                onFriendListChange(user1Id);
            }
        }
        // user2
        {
            uint user2Id = friend.user2Id.v;
            // Find friendSet
            HashSet<Friend> friend2Set = null;
            {
                if (!friendDict.TryGetValue(user2Id, out friend2Set))
                {
                    friend2Set = new HashSet<Friend>();
                    friendDict.Add(user2Id, friend2Set);
                }
            }
            // Add
            friend2Set.Add(friend);
            // delegate
            {
                onFriendListChange(user2Id);
            }
        }
    }

    private void removeFriend(Friend friend)
    {
        // user1
        {
            uint user1Id = friend.user1Id.v;
            HashSet<Friend> friend1Set = null;
            if (friendDict.TryGetValue(user1Id, out friend1Set))
            {
                friend1Set.Remove(friend);
            }
            // delegate
            {
                onFriendListChange(user1Id);
            }
        }
        // user2
        {
            uint user2Id = friend.user2Id.v;
            HashSet<Friend> friend2Set = null;
            if (friendDict.TryGetValue(user2Id, out friend2Set))
            {
                friend2Set.Remove(friend);
            }
            // delegate
            {
                onFriendListChange(user2Id);
            }
        }
    }

    #region implement callBacks

    public void onAddCallBack<T>(T data) where T : Data
    {
        // Debug.LogError("onAddCallBack: " + data + "; " + this);
        if (data is FriendWorld)
        {
            FriendWorld friendWorld = data as FriendWorld;
            // Child
            {
                friendWorld.friends.allAddCallBack(this);
            }
            return;
        }
        // Child
        if (data is Friend)
        {
            Friend friend = data as Friend;
            {
                this.addFriend(friend);
            }
            return;
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public void onRemoveCallBack<T>(T data, bool isHide) where T : Data
    {
        Debug.LogError("onRemoveCallBack: " + data + "; " + this);
        if (data is FriendWorld)
        {
            FriendWorld friendWorld = data as FriendWorld;
            // Child
            {
                friendWorld.friends.allRemoveCallBack(this);
            }
            return;
        }
        // Child
        if (data is Friend)
        {
            Friend friend = data as Friend;
            {
                this.removeFriend(friend);
            }
            return;
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if (wrapProperty.p is FriendWorld)
        {
            switch ((FriendWorld.Property)wrapProperty.n)
            {
                case FriendWorld.Property.friends:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                    }
                    break;
                default:
                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        {
            if (wrapProperty.p is Friend)
            {
                switch ((Friend.Property)wrapProperty.n)
                {
                    case Friend.Property.state:
                        break;
                    case Friend.Property.user1Id:
                        {
                            // Find Friend
                            Friend friend = wrapProperty.p as Friend;
                            // Add
                            if (friend != null)
                            {
                                this.addFriend(friend);
                            }
                            else
                            {
                                Debug.LogError("friend null: " + this);
                            }
                        }
                        break;
                    case Friend.Property.user1:
                        break;
                    case Friend.Property.user2Id:
                        {
                            // Find Friend
                            Friend friend = wrapProperty.p as Friend;
                            // Add
                            if (friend != null)
                            {
                                this.addFriend(friend);
                            }
                            else
                            {
                                Debug.LogError("friend null: " + this);
                            }
                        }
                        break;
                    case Friend.Property.user2:
                        break;
                    case Friend.Property.time:
                        break;
                    case Friend.Property.chatRoom:
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
        }
        Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

    #region interface

    public interface Delegate
    {

        void onFriendListChange(uint userId);

    }

    public List<Delegate> delegates = new List<Delegate>();

    private void onFriendListChange(uint userId)
    {
        for (int i = 0; i < delegates.Count; i++)
        {
            delegates[i].onFriendListChange(userId);
        }
    }

    #endregion

}