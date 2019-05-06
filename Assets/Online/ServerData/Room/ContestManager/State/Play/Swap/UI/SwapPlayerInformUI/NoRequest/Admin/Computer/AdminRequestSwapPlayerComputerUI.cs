﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;

namespace GameManager.Match.Swap
{
    public class AdminRequestSwapPlayerComputerUI : UIBehavior<AdminRequestSwapPlayerComputerUI.UIData>
    {

        #region UIData

        public class UIData : Data
        {

            public VP<ReferenceData<TeamPlayer>> teamPlayer;

            public VP<ComputerUI.UIData> editComputer;

            #region State

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
                teamPlayer,
                editComputer,
                state
            }

            public UIData() : base()
            {
                this.teamPlayer = new VP<ReferenceData<TeamPlayer>>(this, (byte)Property.teamPlayer, new ReferenceData<TeamPlayer>(null));
                // editComputer
                {
                    ComputerUI.UIData editComputerUIData = new ComputerUI.UIData();
                    {
                        editComputerUIData.showType.v = UIRectTransform.ShowType.HeadLess;
                        editComputerUIData.aiUIDataShowType.v = UIRectTransform.ShowType.HeadLess;
                        editComputerUIData.editComputer.v.editType.v = EditType.Later;
                        editComputerUIData.editComputer.v.canEdit.v = true;
                    }
                    this.editComputer = new VP<ComputerUI.UIData>(this, (byte)Property.editComputer, editComputerUIData);
                }
                this.state = new VP<State>(this, (byte)Property.state, State.None);
            }

            #endregion

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
                            AdminRequestSwapPlayerComputerUI adminRequestSwapPlayerComputerUI = this.findCallBack<AdminRequestSwapPlayerComputerUI>();
                            if (adminRequestSwapPlayerComputerUI != null)
                            {
                                isProcess = adminRequestSwapPlayerComputerUI.useShortKey(e);
                            }
                            else
                            {
                                Debug.LogError("adminRequestSwapPlayerComputerUI null: " + this);
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
        private static readonly TxtLanguage txtTitle = new TxtLanguage("Choose Computer");

        private static readonly TxtLanguage txtRequest = new TxtLanguage("Request");
        private static readonly TxtLanguage txtRequesting = new TxtLanguage("Requesting");
        private static readonly TxtLanguage txtCancelRequest = new TxtLanguage("Cancel");

        public Text tvReset;
        private static readonly TxtLanguage txtReset = new TxtLanguage("Reset");

        private static readonly TxtLanguage txtRequestError = new TxtLanguage("Request error");

        static AdminRequestSwapPlayerComputerUI()
        {
            txtTitle.add(Language.Type.vi, "Chọn Máy");
            // request
            {
                txtRequest.add(Language.Type.vi, "Yêu Cầu");
                txtRequesting.add(Language.Type.vi, "Đang gửi");
                txtCancelRequest.add(Language.Type.vi, "Huỷ");
            }
            txtReset.add(Language.Type.vi, "Đặt Lại");
            txtRequestError.add(Language.Type.vi, "Yêu cầu lỗi");
        }

        #endregion

        #region Refresh

        public Button btnRequest;
        public Text tvRequest;

        private bool needReset = false;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // Reset
                    {
                        if (needReset)
                        {
                            needReset = false;
                            this.data.reset();
                        }
                    }
                    TeamPlayer teamPlayer = this.data.teamPlayer.v.data;
                    if (teamPlayer != null)
                    {
                        // editComputer
                        {
                            ComputerUI.UIData editComputer = this.data.editComputer.v;
                            if (editComputer != null)
                            {
                                // Find origin
                                Computer originComputer = null;
                                {
                                    // find from teamPlayer
                                    if (teamPlayer.inform.v is Computer)
                                    {
                                        originComputer = teamPlayer.inform.v as Computer;
                                        editComputer.editComputer.v.compare.v = new ReferenceData<Computer>(originComputer);
                                    }
                                    // make new
                                    if (originComputer == null)
                                    {
                                        editComputer.editComputer.v.compare.v = new ReferenceData<Computer>(null);
                                        // Find
                                        {
                                            // find old
                                            if (editComputer.editComputer.v.origin.v.data != null)
                                            {
                                                originComputer = editComputer.editComputer.v.origin.v.data;
                                            }
                                            // Make new
                                            if (originComputer == null)
                                            {
                                                originComputer = new Computer();
                                                // name
                                                {
                                                    int playerIndex = teamPlayer.playerIndex.v;
                                                    int teamIndex = 0;
                                                    {
                                                        MatchTeam matchTeam = teamPlayer.findDataInParent<MatchTeam>();
                                                        if (matchTeam != null)
                                                        {
                                                            teamIndex = matchTeam.teamIndex.v;
                                                        }
                                                        else
                                                        {
                                                            Debug.LogError("matchTeam null: " + this);
                                                        }
                                                    }
                                                    originComputer.computerName.v = "Player " + playerIndex + ", Team: " + teamIndex;
                                                }
                                            }
                                        }
                                        // Update
                                        {
                                            // ai
                                            {
                                                // find current gameType
                                                GameType.Type gameTypeType = GameType.Type.Xiangqi;
                                                {
                                                    ContestManagerStatePlay contestManagerStatePlay = teamPlayer.findDataInParent<ContestManagerStatePlay>();
                                                    if (contestManagerStatePlay != null)
                                                    {
                                                        gameTypeType = contestManagerStatePlay.gameTypeType.v;
                                                    }
                                                    else
                                                    {
                                                        Debug.LogError("contestManagerStatePlay null: " + this);
                                                    }
                                                }
                                                // Process
                                                {
                                                    bool needNew = true;
                                                    {
                                                        if (originComputer.ai.v != null)
                                                        {
                                                            if (originComputer.ai.v.getType() == gameTypeType)
                                                            {
                                                                needNew = false;
                                                            }
                                                        }
                                                    }
                                                    if (needNew)
                                                    {
                                                        Computer.AI ai = Computer.AI.makeDefaultAI(gameTypeType);
                                                        {
                                                            ai.uid = originComputer.ai.makeId();
                                                        }
                                                        originComputer.ai.v = ai;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                // Set
                                editComputer.editComputer.v.origin.v = new ReferenceData<Computer>(originComputer);
                            }
                            else
                            {
                                Debug.LogError("editComputer null: " + this);
                            }
                        }
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
                                        if (Server.IsServerOnline(teamPlayer))
                                        {
                                            // Find Computer
                                            Computer computer = this.data.editComputer.v.editComputer.v.show.v.data;
                                            // Process
                                            if (computer != null)
                                            {
                                                // find swap
                                                Swap swap = null;
                                                {
                                                    ContestManagerStatePlay contestManagerStatePlay = teamPlayer.findDataInParent<ContestManagerStatePlay>();
                                                    if (contestManagerStatePlay != null)
                                                    {
                                                        swap = contestManagerStatePlay.swap.v;
                                                    }
                                                    else
                                                    {
                                                        Debug.LogError("contestManagerStatePlay null: " + this);
                                                    }
                                                }
                                                if (swap != null)
                                                {
                                                    int playerIndex = teamPlayer.playerIndex.v;
                                                    // find teamIndex
                                                    int teamIndex = 0;
                                                    {
                                                        MatchTeam matchTeam = teamPlayer.findDataInParent<MatchTeam>();
                                                        if (matchTeam != null)
                                                        {
                                                            teamIndex = matchTeam.teamIndex.v;
                                                        }
                                                        else
                                                        {
                                                            Debug.LogError("matchTeam null: " + this);
                                                        }
                                                    }
                                                    swap.requestChangeComputer(Server.getProfileUserId(teamPlayer), teamIndex, playerIndex, computer);
                                                }
                                                else
                                                {
                                                    Debug.LogError("swap null: " + this);
                                                }
                                                this.data.state.v = UIData.State.Wait;
                                            }
                                            else
                                            {
                                                Debug.LogError("computer null: " + this);
                                                this.data.state.v = UIData.State.None;
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("server is not online: " + this);
                                        }
                                    }
                                    break;
                                case UIData.State.Wait:
                                    {
                                        if (Server.IsServerOnline(teamPlayer))
                                        {
                                            startRoutine(ref this.wait, TaskWait());
                                        }
                                        else
                                        {
                                            Debug.LogError("server offline: " + this);
                                            this.data.state.v = UIData.State.None;
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
                                            // Find
                                            bool isDiffrent = true;
                                            {
                                                if (teamPlayer.inform.v is Computer)
                                                {
                                                    Computer currentComputer = teamPlayer.inform.v as Computer;
                                                    // Get show Computer
                                                    Computer showComputer = this.data.editComputer.v.editComputer.v.show.v.data;
                                                    if (showComputer != null)
                                                    {
                                                        if (!DataUtils.IsDifferent(currentComputer, showComputer))
                                                        {
                                                            isDiffrent = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Debug.LogError("showComputer null: " + this);
                                                        isDiffrent = false;
                                                    }
                                                }
                                            }
                                            // Process
                                            if (isDiffrent)
                                            {
                                                btnRequest.interactable = true;
                                                tvRequest.text = txtRequest.get();
                                            }
                                            else
                                            {
                                                btnRequest.interactable = false;
                                                tvRequest.text = txtRequest.get();// "Not Different, Cannot Request";
                                            }
                                        }
                                        break;
                                    case UIData.State.Request:
                                        {
                                            btnRequest.interactable = true;
                                            tvRequest.text = txtCancelRequest.get();// "Cancel Request?";
                                        }
                                        break;
                                    case UIData.State.Wait:
                                        {
                                            btnRequest.interactable = false;
                                            tvRequest.text = txtRequesting.get();// "Requesting...";
                                        }
                                        break;
                                    default:
                                        Debug.LogError("unknown state: " + this.data.state.v + "; " + this);
                                        break;
                                }
                            }
                            else
                            {
                                Debug.LogError("btnRequest, tvRequest null: " + this);
                            }
                        }
                        // txt
                        {
                            if (lbTitle != null)
                            {
                                lbTitle.text = txtTitle.get();
                            }
                            else
                            {
                                Debug.LogError("lbTitle null");
                            }
                            if (tvReset != null)
                            {
                                tvReset.text = txtReset.get();
                            }
                            else
                            {
                                Debug.LogError("tvReset null");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("teamPlayer null: " + this);
                    }
                }
                else
                {
                    // Debug.LogError ("data null: " + this);
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
                // Chuyen sang state none
                {
                    if (this.data != null)
                    {
                        this.data.state.v = UIData.State.None;
                    }
                    else
                    {
                        Debug.LogError("data null: " + this);
                    }
                }
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

        public ComputerUI editComputerPrefab;
        public Transform editComputerContainer;

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
                    uiData.teamPlayer.allAddCallBack(this);
                    uiData.editComputer.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Setting
            if(data is Setting)
            {
                dirty = true;
                return;
            }
            // Child
            {
                // TeamPlayer
                {
                    if (data is TeamPlayer)
                    {
                        TeamPlayer teamPlayer = data as TeamPlayer;
                        // Parent
                        {
                            DataUtils.addParentCallBack(teamPlayer, this, ref this.server);
                        }
                        // Child
                        {
                            teamPlayer.inform.allAddCallBack(this);
                        }
                        dirty = true;
                        needReset = true;
                        return;
                    }
                    // Parent
                    if (data is Server)
                    {
                        dirty = true;
                        return;
                    }
                    // Child
                    {
                        if (data is GamePlayer.Inform)
                        {
                            GamePlayer.Inform inform = data as GamePlayer.Inform;
                            // UI
                            {
                                switch (inform.getType())
                                {
                                    case GamePlayer.Inform.Type.None:
                                        break;
                                    case GamePlayer.Inform.Type.Human:
                                        break;
                                    case GamePlayer.Inform.Type.Computer:
                                        {
                                            Computer computer = inform as Computer;
                                            computer.ai.allAddCallBack(this);
                                        }
                                        break;
                                    default:
                                        Debug.LogError("unknown type: " + inform.getType() + "; " + this);
                                        break;
                                }
                            }
                            dirty = true;
                            return;
                        }
                    }
                }
                // editComputer
                {
                    if (data is ComputerUI.UIData)
                    {
                        ComputerUI.UIData editComputerUIData = data as ComputerUI.UIData;
                        // UI
                        {
                            UIUtils.Instantiate(editComputerUIData, editComputerPrefab, editComputerContainer);
                        }
                        // Child
                        {
                            editComputerUIData.editComputer.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    {
                        if (data is EditData<Computer>)
                        {
                            EditData<Computer> editComputer = data as EditData<Computer>;
                            // Child
                            {
                                editComputer.show.allAddCallBack(this);
                            }
                            dirty = true;
                            return;
                        }
                    }
                }
                if (data is Computer.AI)
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
                    uiData.teamPlayer.allRemoveCallBack(this);
                    uiData.editComputer.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if(data is Setting)
            {
                return;
            }
            // Child
            {
                // TeamPlayer
                {
                    if (data is TeamPlayer)
                    {
                        TeamPlayer teamPlayer = data as TeamPlayer;
                        // Parent
                        {
                            DataUtils.removeParentCallBack(teamPlayer, this, ref this.server);
                        }
                        // Child
                        {
                            teamPlayer.inform.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Parent
                    if (data is Server)
                    {
                        return;
                    }
                    // Child
                    {
                        if (data is GamePlayer.Inform)
                        {
                            GamePlayer.Inform inform = data as GamePlayer.Inform;
                            // UI
                            {
                                switch (inform.getType())
                                {
                                    case GamePlayer.Inform.Type.None:
                                        break;
                                    case GamePlayer.Inform.Type.Human:
                                        break;
                                    case GamePlayer.Inform.Type.Computer:
                                        {
                                            Computer computer = inform as Computer;
                                            computer.ai.allRemoveCallBack(this);
                                        }
                                        break;
                                    default:
                                        Debug.LogError("unknown type: " + inform.getType() + "; " + this);
                                        break;
                                }
                            }
                            return;
                        }
                    }
                }
                // editComputer
                {
                    if (data is ComputerUI.UIData)
                    {
                        ComputerUI.UIData editComputerUIData = data as ComputerUI.UIData;
                        // UI
                        {
                            editComputerUIData.removeCallBackAndDestroy(typeof(ComputerUI));
                        }
                        // Child
                        {
                            editComputerUIData.editComputer.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    {
                        if (data is EditData<Computer>)
                        {
                            EditData<Computer> editComputer = data as EditData<Computer>;
                            // Child
                            {
                                editComputer.show.allRemoveCallBack(this);
                            }
                            return;
                        }
                    }
                }
                if (data is Computer.AI)
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
                    case UIData.Property.teamPlayer:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.editComputer:
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
            if(wrapProperty.p is Setting)
            {
                switch ((Setting.Property)wrapProperty.n)
                {
                    case Setting.Property.language:
                        dirty = true;
                        break;
                    case Setting.Property.style:
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
                // TeamPlayer
                {
                    if (wrapProperty.p is TeamPlayer)
                    {
                        switch ((TeamPlayer.Property)wrapProperty.n)
                        {
                            case TeamPlayer.Property.playerIndex:
                                break;
                            case TeamPlayer.Property.inform:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                    needReset = true;
                                }
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Parent
                    if (wrapProperty.p is Server)
                    {
                        Server.State.OnUpdateSyncStateChange(wrapProperty, this);
                        return;
                    }
                    // Child
                    {
                        if (wrapProperty.p is GamePlayer.Inform)
                        {
                            GamePlayer.Inform inform = wrapProperty.p as GamePlayer.Inform;
                            // UI
                            {
                                switch (inform.getType())
                                {
                                    case GamePlayer.Inform.Type.None:
                                        break;
                                    case GamePlayer.Inform.Type.Human:
                                        break;
                                    case GamePlayer.Inform.Type.Computer:
                                        {
                                            switch ((Computer.Property)wrapProperty.n)
                                            {
                                                case Computer.Property.computerName:
                                                    dirty = true;
                                                    needReset = true;
                                                    break;
                                                case Computer.Property.avatarUrl:
                                                    dirty = true;
                                                    needReset = true;
                                                    break;
                                                case Computer.Property.ai:
                                                    dirty = true;
                                                    needReset = true;
                                                    break;
                                                default:
                                                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                                    break;
                                            }
                                        }
                                        break;
                                    default:
                                        Debug.LogError("unknown type: " + inform.getType() + "; " + this);
                                        break;
                                }
                            }
                            return;
                        }
                    }
                }
                // editComputer
                {
                    if (wrapProperty.p is ComputerUI.UIData)
                    {
                        switch ((ComputerUI.UIData.Property)wrapProperty.n)
                        {
                            case ComputerUI.UIData.Property.editComputer:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                    needReset = true;
                                }
                                break;
                            case ComputerUI.UIData.Property.name:
                                break;
                            case ComputerUI.UIData.Property.avatarUrl:
                                break;
                            case ComputerUI.UIData.Property.aiUIData:
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    {
                        if (wrapProperty.p is EditData<Computer>)
                        {
                            switch ((EditData<Computer>.Property)wrapProperty.n)
                            {
                                case EditData<Computer>.Property.origin:
                                    break;
                                case EditData<Computer>.Property.show:
                                    {
                                        ValueChangeUtils.replaceCallBack(this, syncs);
                                        dirty = true;
                                        needReset = true;
                                    }
                                    break;
                                case EditData<Computer>.Property.compare:
                                    break;
                                case EditData<Computer>.Property.compareOtherType:
                                    break;
                                case EditData<Computer>.Property.canEdit:
                                    break;
                                case EditData<Computer>.Property.editType:
                                    break;
                                default:
                                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                    break;
                            }
                            return;
                        }
                    }
                }
                if (wrapProperty.p is Computer.AI)
                {
                    dirty = true;
                    needReset = true;
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
                        case KeyCode.R:
                            {
                                if (btnReset != null && btnReset.gameObject.activeInHierarchy && btnReset.interactable)
                                {
                                    this.onClickBtnReset();
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
                        {
                            this.data.state.v = UIData.State.Request;
                        }
                        break;
                    case UIData.State.Request:
                        {
                            this.data.state.v = UIData.State.None;
                        }
                        break;
                    case UIData.State.Wait:
                        {
                            Debug.LogError("you are requesting: " + this);
                        }
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

        public Button btnReset;

        [UnityEngine.Scripting.Preserve]
        public void onClickBtnReset()
        {
            if (this.data != null)
            {
                ComputerUI.UIData editComputerUIData = this.data.editComputer.v;
                if (editComputerUIData != null)
                {
                    EditData<Computer> editComputer = editComputerUIData.editComputer.v;
                    if (editComputer != null)
                    {
                        editComputer.show.v = new ReferenceData<Computer>(null);
                    }
                    else
                    {
                        Debug.LogError("editComputer null: " + this);
                    }
                }
                else
                {
                    Debug.LogError("editComputerUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

    }
}