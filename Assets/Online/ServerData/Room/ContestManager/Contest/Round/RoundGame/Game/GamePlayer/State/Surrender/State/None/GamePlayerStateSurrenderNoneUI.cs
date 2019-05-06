﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;

public class GamePlayerStateSurrenderNoneUI : UIHaveTransformDataBehavior<GamePlayerStateSurrenderNoneUI.UIData>
{

    #region UIData

    public class UIData : GamePlayerStateSurrenderUI.UIData.Sub
    {

        public VP<ReferenceData<GamePlayerStateSurrenderNone>> none;

        #region state

        public enum State
        {
            None,
            Request,
            Wait
        }

        public VP<State> state;

        #endregion

        #region Constructor

        public enum Property
        {
            none,
            state
        }

        public UIData()
        {
            this.none = new VP<ReferenceData<GamePlayerStateSurrenderNone>>(this, (byte)Property.none, new ReferenceData<GamePlayerStateSurrenderNone>(null));
            this.state = new VP<State>(this, (byte)Property.state, State.None);
        }

        #endregion

        public override GamePlayerStateSurrender.State.Type getType()
        {
            return GamePlayerStateSurrender.State.Type.None;
        }

        public void reset()
        {
            this.state.v = State.None;
        }

        public bool processEvent(Event e)
        {
            bool isProcess = false;
            {
                // shortKey
                if (!isProcess)
                {
                    if (Setting.get().useShortKey.v)
                    {
                        GamePlayerStateSurrenderNoneUI gamePlayerStateSurrenderNoneUI = this.findCallBack<GamePlayerStateSurrenderNoneUI>();
                        if (gamePlayerStateSurrenderNoneUI != null)
                        {
                            isProcess = gamePlayerStateSurrenderNoneUI.useShortKey(e);
                        }
                        else
                        {
                            Debug.LogError("gamePlayerStateSurrenderNoneUI null: " + this);
                        }
                    }
                }
            }
            return isProcess;
        }

    }

    #endregion

    #region txt

    public Text lbTitle;
    private static readonly TxtLanguage txtTitle = new TxtLanguage("Do you want to stop surrendering?");

    private static readonly TxtLanguage txtRequest = new TxtLanguage("Stop");
    private static readonly TxtLanguage txtCancelRequest = new TxtLanguage("Cancel stop?");
    private static readonly TxtLanguage txtRequesting = new TxtLanguage("Stopping");
    private static readonly TxtLanguage txtCannotRequest = new TxtLanguage("Can't stop");

    private static readonly TxtLanguage txtRequestError = new TxtLanguage("Send request to stop surrendering error");

    static GamePlayerStateSurrenderNoneUI()
    {
        txtTitle.add(Language.Type.vi, "Bạn có muốn dừng đầu hàng không?");

        txtRequest.add(Language.Type.vi, "Dừng");
        txtCancelRequest.add(Language.Type.vi, "Huỷ dừng?");
        txtRequesting.add(Language.Type.vi, "Đang dừng");
        txtCannotRequest.add(Language.Type.vi, "Không thể dừng");

        txtRequestError.add(Language.Type.vi, "Gửi yêu cầu dừng đầu hàng lỗi");
    }

    #endregion

    #region Refresh

    public Button btnRequest;
    public Text tvRequest;

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                GamePlayerStateSurrenderNone none = this.data.none.v.data;
                if (none != null)
                {
                    // title
                    if (lbTitle != null)
                    {
                        lbTitle.text = txtTitle.get();
                    }
                    else
                    {
                        Debug.LogError("lbTitle null");
                    }
                    // btn, Task
                    uint profileId = Server.getProfileUserId(none);
                    if (none.isCanRequestCancel(profileId))
                    {
                        // Task
                        {
                            switch (this.data.state.v)
                            {
                                case UIData.State.None:
                                    {
                                        destroyRoutine(wait);
                                    }
                                    break;
                                case UIData.State.Request:
                                    {
                                        destroyRoutine(wait);
                                        if (Server.IsServerOnline(none))
                                        {
                                            none.requestMakeRequestCancel(profileId);
                                            this.data.state.v = UIData.State.Wait;
                                        }
                                        else
                                        {
                                            Debug.LogError("server not online: " + this);
                                        }
                                    }
                                    break;
                                case UIData.State.Wait:
                                    {
                                        if (Server.IsServerOnline(none))
                                        {
                                            startRoutine(ref this.wait, TaskWait());
                                        }
                                        else
                                        {
                                            this.data.state.v = UIData.State.None;
                                            destroyRoutine(wait);
                                        }
                                    }
                                    break;
                                default:
                                    Debug.LogError("unknown state: " + this.data.state.v + "; " + this);
                                    break;
                            }
                        }
                        // UI
                        {
                            if (btnRequest != null && tvRequest != null)
                            {
                                switch (this.data.state.v)
                                {
                                    case UIData.State.None:
                                        {
                                            btnRequest.interactable = true;
                                            tvRequest.text = txtRequest.get();
                                        }
                                        break;
                                    case UIData.State.Request:
                                        {
                                            btnRequest.interactable = true;
                                            tvRequest.text = txtCancelRequest.get();
                                        }
                                        break;
                                    case UIData.State.Wait:
                                        {
                                            btnRequest.interactable = false;
                                            tvRequest.text = txtRequesting.get();
                                        }
                                        break;
                                    default:
                                        Debug.LogError("unknowns state: " + this.data.state.v + "; " + this);
                                        break;
                                }
                            }
                            else
                            {
                                Debug.LogError("btnRequest, tvRequest null: " + this);
                            }
                        }
                    }
                    else
                    {
                        // Task
                        {
                            this.data.state.v = UIData.State.None;
                            destroyRoutine(wait);
                        }
                        // UI
                        {
                            if (btnRequest != null && tvRequest != null)
                            {
                                btnRequest.interactable = false;
                                tvRequest.text = txtCannotRequest.get();
                            }
                            else
                            {
                                Debug.LogError("btnRequest, tvRequest null: " + this);
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("none null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return false;
    }

    #endregion

    #region Task wait

    private Routine wait;

    public IEnumerator TaskWait()
    {
        if (this.data != null)
        {
            yield return new Wait(Global.WaitSendTime);
            this.data.state.v = UIData.State.None;
            Toast.showMessage(txtRequestError.get());
            Debug.LogError("request error: " + this);
        }
        else
        {
            Debug.LogError("data null: " + this);
        }
    }

    public override List<Routine> getRoutineList()
    {
        List<Routine> ret = new List<Routine>();
        {
            ret.Add(wait);
        }
        return ret;
    }

    #endregion

    #region implement callBacks

    public RoomCheckChangeAdminChange<GamePlayerStateSurrenderNone> roomCheckAdminChange = new RoomCheckChangeAdminChange<GamePlayerStateSurrenderNone>();
    public GameCheckPlayerChange<GamePlayerStateSurrenderNone> gameCheckPlayerChange = new GameCheckPlayerChange<GamePlayerStateSurrenderNone>();

    private Server server = null;

    public override void onAddCallBack<T>(T data)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Setting
            Setting.get().addCallBack(this);
            // Child
            {
                uiData.none.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Setting
        if (data is Setting)
        {
            dirty = true;
            return;
        }
        // Child
        {
            if (data is GamePlayerStateSurrenderNone)
            {
                GamePlayerStateSurrenderNone none = data as GamePlayerStateSurrenderNone;
                // reset
                {
                    if (this.data != null)
                    {
                        this.data.reset();
                    }
                    else
                    {
                        Debug.LogError("data null: " + this);
                    }
                }
                // CheckChange
                {
                    // admin
                    {
                        roomCheckAdminChange.addCallBack(this);
                        roomCheckAdminChange.setData(none);
                    }
                    // player
                    {
                        gameCheckPlayerChange.addCallBack(this);
                        gameCheckPlayerChange.setData(none);
                    }
                }
                // Parent
                {
                    DataUtils.addParentCallBack(none, this, ref this.server);
                }
                dirty = true;
                return;
            }
            // CheckChange
            {
                if (data is RoomCheckChangeAdminChange<GamePlayerStateSurrenderNone>)
                {
                    dirty = true;
                    return;
                }
                if (data is GameCheckPlayerChange<GamePlayerStateSurrenderNone>)
                {
                    dirty = true;
                    return;
                }
            }
            // Parent
            if (data is Server)
            {
                dirty = true;
                return;
            }
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Setting
            Setting.get().removeCallBack(this);
            // Child
            {
                uiData.none.allRemoveCallBack(this);
            }
            this.setDataNull(uiData);
            return;
        }
        // Setting
        if (data is Setting)
        {
            return;
        }
        // Child
        {
            if (data is GamePlayerStateSurrenderNone)
            {
                GamePlayerStateSurrenderNone none = data as GamePlayerStateSurrenderNone;
                // CheckChange
                {
                    // admin
                    {
                        roomCheckAdminChange.removeCallBack(this);
                        roomCheckAdminChange.setData(null);
                    }
                    // player
                    {
                        gameCheckPlayerChange.removeCallBack(this);
                        gameCheckPlayerChange.setData(null);
                    }
                }
                // Parent
                {
                    DataUtils.removeParentCallBack(none, this, ref this.server);
                }
                return;
            }
            // CheckChange
            {
                if (data is RoomCheckChangeAdminChange<GamePlayerStateSurrenderNone>)
                {
                    return;
                }
                if (data is GameCheckPlayerChange<GamePlayerStateSurrenderNone>)
                {
                    return;
                }
            }
            // Parent
            if (data is Server)
            {
                return;
            }
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if (wrapProperty.p is UIData)
        {
            switch ((UIData.Property)wrapProperty.n)
            {
                case UIData.Property.none:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                case UIData.Property.state:
                    dirty = true;
                    break;
                default:
                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Setting
        if (wrapProperty.p is Setting)
        {
            switch ((Setting.Property)wrapProperty.n)
            {
                case Setting.Property.language:
                    dirty = true;
                    break;
                case Setting.Property.showLastMove:
                    break;
                case Setting.Property.viewUrlImage:
                    break;
                case Setting.Property.animationSetting:
                    break;
                case Setting.Property.maxThinkCount:
                    break;
                default:
                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        {
            if (wrapProperty.p is GamePlayerStateSurrenderNone)
            {
                switch ((GamePlayerStateSurrenderNone.Property)wrapProperty.n)
                {
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // CheckChange
            {
                if (wrapProperty.p is RoomCheckChangeAdminChange<GamePlayerStateSurrenderNone>)
                {
                    dirty = true;
                    return;
                }
                if (wrapProperty.p is GameCheckPlayerChange<GamePlayerStateSurrenderNone>)
                {
                    dirty = true;
                    return;
                }
            }
            // Parent
            if (wrapProperty.p is Server)
            {
                Server.State.OnUpdateSyncStateChange(wrapProperty, this);
                return;
            }
        }
        Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

    public bool useShortKey(Event e)
    {
        bool isProcess = false;
        {
            if (e.isKey && e.type == EventType.KeyUp)
            {
                switch (e.keyCode)
                {
                    case KeyCode.Q:
                        {
                            if (btnRequest != null && btnRequest.gameObject.activeInHierarchy && btnRequest.interactable)
                            {
                                this.onClickBtnRequest();
                                isProcess = true;
                            }
                            else
                            {
                                Debug.LogError("cannot click");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        return isProcess;
    }

    [UnityEngine.Scripting.Preserve]
    public void onClickBtnRequest()
    {
        if (this.data != null)
        {
            switch (this.data.state.v)
            {
                case UIData.State.None:
                    this.data.state.v = UIData.State.Request;
                    break;
                case UIData.State.Request:
                    this.data.state.v = UIData.State.None;
                    break;
                case UIData.State.Wait:
                    Debug.LogError("You are requesting: " + this);
                    break;
                default:
                    Debug.LogError("unknown state: " + this.data.state.v + "; " + this);
                    break;
            }
        }
        else
        {
            Debug.LogError("data null: " + this);
        }
    }

}