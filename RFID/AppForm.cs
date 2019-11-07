using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Symbol.RFID3;
using System.IO;

namespace RFID
{
    public partial class AppForm : Form
    {
        internal bool m_IsConnected;
        internal GPIs input;
        internal RFIDReader m_ReaderAPI;
        internal ConnectForm connectForm;

        internal string m_SelectedTagID = "";
        internal ArrayList m_GPIStateList;

        private Hashtable m_TagTable;
        private uint m_TagTotalCount;

        internal class AccessOperationResult
        {
            public RFIDResults m_Result;
            public String m_VendorMessage;
            public String m_StatusDescription;
            public ACCESS_OPERATION_CODE m_OpCode;

            public AccessOperationResult()
            {
                m_Result = RFIDResults.RFID_NO_ACCESS_IN_PROGRESS;
                m_OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ;
            }
        }
        public AppForm()
        {
            InitializeComponent();
            m_TagTotalCount = 0;
        }

        private void myUpdateStatus(Events.StatusEventData eventData)
        {
            switch (eventData.StatusEventType)
            {
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.INVENTORY_START_EVENT:
                    functionCallStatusLabel.Text = "Inventory started";
                    this.readBtn.Enabled = true;
                    this.readBtn.Text = "Stop Reading";

                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.INVENTORY_STOP_EVENT:
                    functionCallStatusLabel.Text = "Inventory stopped";
                    this.readBtn.Enabled = true;
                    this.readBtn.Text = "Start Reading";
                    //memBank_CB.Enabled = true;

                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ACCESS_START_EVENT:
                    functionCallStatusLabel.Text = "Access Operation started";
                    this.readBtn.Enabled = true;
                    this.readBtn.Text = "Stop Reading";
                    //memBank_CB.Enabled = false; ;
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ACCESS_STOP_EVENT:
                    functionCallStatusLabel.Text = "Access Operation stopped";

                    if (this.m_SelectedTagID == string.Empty)
                    {
                        uint successCount, failureCount;
                        successCount = failureCount = 0;
                        m_ReaderAPI.Actions.TagAccess.GetLastAccessResult(ref successCount, ref failureCount);
                        functionCallStatusLabel.Text = "Access completed - Success Count: " + successCount.ToString()
                            + ", Failure Count: " + failureCount.ToString();
                    }
                    //resetButtonState();
                    this.readBtn.Enabled = true;
                    this.readBtn.Text = "Start Reading";
                    //memBank_CB.Enabled = true;
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.GPI_EVENT:
                    this.updateGPIState();
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ANTENNA_EVENT:
                    string status = (eventData.AntennaEventData.AntennaEvent == ANTENNA_EVENT_TYPE.ANTENNA_CONNECTED ? "connected" : "disconnected");
                    functionCallStatusLabel.Text = "Antenna " + eventData.AntennaEventData.AntennaID.ToString() + " has been " + status;
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.BUFFER_FULL_WARNING_EVENT:
                    functionCallStatusLabel.Text = " Buffer full warning";
                    myUpdateRead(null);
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.BUFFER_FULL_EVENT:
                    functionCallStatusLabel.Text = "Buffer full";
                    myUpdateRead(null);
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.DISCONNECTION_EVENT:
                    functionCallStatusLabel.Text = "Disconnection Event " + eventData.DisconnectionEventData.DisconnectEventInfo.ToString();
                    
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.READER_EXCEPTION_EVENT:
                    functionCallStatusLabel.Text = "Reader ExceptionEvent " + eventData.ReaderExceptionEventData.ReaderExceptionEventInfo.ToString();
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.TEMPERATURE_ALARM_EVENT:
                    functionCallStatusLabel.Text = "Temperature Alarm " + eventData.TemperatureAlarmEventData.SourceName.ToString() + " Temperature " + eventData.TemperatureAlarmEventData.CurrentTemperature.ToString() + " Level " + eventData.TemperatureAlarmEventData.AlarmLevel.ToString();
                    break;
                default:
                    break;
            }
        }

        private void myUpdateRead(Events.ReadEventData eventData)
        {
            Symbol.RFID3.TagData[] tagData = m_ReaderAPI.Actions.GetReadTags(1000);
            if (tagData != null)
            {
                for (int nIndex = 0; nIndex < tagData.Length; nIndex++)
                {

                    if (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_NONE ||
                        (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ &&
                        tagData[nIndex].OpStatus == ACCESS_OPERATION_STATUS.ACCESS_SUCCESS))
                    {
                        Symbol.RFID3.TagData tag = tagData[nIndex];
                        string tagID = tag.TagID;
                        bool isFound = false;

                        //lock (m_TagTable.SyncRoot)
                        //{
                        //    isFound = m_TagTable.ContainsKey(tagID);
                        //    if (!isFound && this.memBank_CB.SelectedIndex >= 1)
                        //    {
                        //        tagID = tag.TagID + tag.MemoryBank.ToString() + tag.MemoryBankDataOffset.ToString();

                        //        isFound = m_TagTable.ContainsKey(tagID);
                        //    }
                        //}

                        if (isFound)
                        {
                            uint count = 0;
                            ListViewItem item = (ListViewItem)m_TagTable[tagID];
                            try
                            {
                                count = uint.Parse(item.SubItems[3].Text) + tagData[nIndex].TagSeenCount;
                                m_TagTotalCount += tagData[nIndex].TagSeenCount;
                            }
                            catch (FormatException fe)
                            {
                                functionCallStatusLabel.Text = fe.Message;
                                break;
                            }
                            item.SubItems[1].Text = tag.TagID.ToString();
                            item.SubItems[2].Text = System.DateTime.Now.ToString();


                        }
                        else
                        {
                            ListViewItem item = new ListViewItem(tag.TagID);
                            // 1 - tag event
                            ListViewItem.ListViewSubItem subItem;
                            subItem = new ListViewItem.ListViewSubItem(item, System.DateTime.Now.ToString());
                            item.SubItems.Add(subItem);
                            // 2 - antenna ID


                            //if (memBank_CB.SelectedIndex >= 1)
                            //{
                            //    // 7 - Memory bank data
                            //    subItem = new ListViewItem.ListViewSubItem(item, tag.MemoryBankData);
                            //    item.SubItems.Add(subItem);

                            //    string memoryBank = tag.MemoryBank.ToString();
                            //    int index = memoryBank.LastIndexOf('_');
                            //    if (index != -1)
                            //    {
                            //        memoryBank = memoryBank.Substring(index + 1);
                            //    }

                            //    // 8 - Memory Bank
                            //    subItem = new ListViewItem.ListViewSubItem(item, memoryBank);
                            //    item.SubItems.Add(subItem);

                            //    // 9 - memory bank offset
                            //    subItem = new ListViewItem.ListViewSubItem(item, tag.MemoryBankDataOffset.ToString());
                            //    item.SubItems.Add(subItem);
                            //}
                            //else
                            //{
                            //    subItem = new ListViewItem.ListViewSubItem(item, "");
                            //    item.SubItems.Add(subItem);
                            //    subItem = new ListViewItem.ListViewSubItem(item, "");
                            //    item.SubItems.Add(subItem);
                            //    subItem = new ListViewItem.ListViewSubItem(item, "");
                            //    item.SubItems.Add(subItem);
                            //}

                            inventoryList.BeginUpdate();
                            inventoryList.Items.Add(item);
                            inventoryList.EndUpdate();

                            lock (m_TagTable.SyncRoot)
                            {
                                m_TagTable.Add(tagID, item);
                            }
                        }
                    }
                }

            }
        }

        private void updateGPIState()
        {
            try
            {
                if (m_IsConnected)
                {
                    for (int index = 0; index < 8; index++)
                    {
                        if (index < m_ReaderAPI.ReaderCapabilities.NumGPIPorts)
                        {
                            Label gpiLabel = (Label)m_GPIStateList[index];
                            GPIs.GPI_PORT_STATE portState = m_ReaderAPI.Config.GPI[index + 1].PortState;

                            if (portState == GPIs.GPI_PORT_STATE.GPI_PORT_STATE_HIGH)
                            {
                                gpiLabel.BackColor = System.Drawing.Color.GreenYellow;
                            }
                            else if (portState == GPIs.GPI_PORT_STATE.GPI_PORT_STATE_LOW)
                            {
                                gpiLabel.BackColor = System.Drawing.Color.Red;
                            }
                            else if (portState == GPIs.GPI_PORT_STATE.GPI_PORT_STATE_UNKNOWN)
                            {

                            }
                        }
                    }
                }
                else
                {
                    for (int index = 0; index < 8; index++)
                    {
                        ((Label)m_GPIStateList[index]).BackColor = System.Drawing.Color.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {
                functionCallStatusLabel.Text = ex.Message;
            }
        }

        

        //private void ConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        //{
            
        //}

        private void Button1_Click(object sender, EventArgs e)
        {
            
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                m_AccessOpResult.m_OpCode = (ACCESS_OPERATION_CODE)accessEvent.Argument;
                m_AccessOpResult.m_Result = RFIDResults.RFID_API_SUCCESS;

                if ((ACCESS_OPERATION_CODE)accessEvent.Argument == ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ)
                {
                    if (m_SelectedTagID != String.Empty)
                    {
                        m_ReadTag = m_ReaderAPI.Actions.TagAccess.ReadWait(
                        m_SelectedTagID, m_ReadForm.m_ReadParams, null);
                    }
                    else
                    {
                        functionCallStatusLabel.Text = "Enter Tag-Id";
                    }
                }
                else if ((ACCESS_OPERATION_CODE)accessEvent.Argument == ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE)
                {
                    if (m_SelectedTagID != String.Empty)
                    {
                        m_ReaderAPI.Actions.TagAccess.WriteWait(
                            m_SelectedTagID, m_WriteForm.m_WriteParams, null);
                    }
                    else
                    {
                        functionCallStatusLabel.Text = "Enter Tag-Id";
                    }
                }
                else if ((ACCESS_OPERATION_CODE)accessEvent.Argument == ACCESS_OPERATION_CODE.ACCESS_OPERATION_LOCK)
                {
                    if (m_SelectedTagID != String.Empty)
                    {
                        m_ReaderAPI.Actions.TagAccess.LockWait(
                            m_SelectedTagID, m_LockForm.m_LockParams, null);
                    }
                    else
                    {
                        functionCallStatusLabel.Text = "Enter Tag-Id";
                    }
                }
                else if ((ACCESS_OPERATION_CODE)accessEvent.Argument == ACCESS_OPERATION_CODE.ACCESS_OPERATION_KILL)
                {
                    if (m_SelectedTagID != String.Empty)
                    {
                        m_ReaderAPI.Actions.TagAccess.KillWait(
                            m_SelectedTagID, m_KillForm.m_KillParams, null);
                    }
                }
                else if ((ACCESS_OPERATION_CODE)accessEvent.Argument == ACCESS_OPERATION_CODE.ACCESS_OPERATION_BLOCK_ERASE)
                {
                    if (m_SelectedTagID != String.Empty)
                    {
                        m_ReaderAPI.Actions.TagAccess.BlockEraseWait(
                            m_SelectedTagID, m_BlockEraseForm.m_BlockEraseParams, null);
                    }
                    else
                    {
                        functionCallStatusLabel.Text = "Enter Tag-Id";
                    }
                }
            }
            catch (OperationFailureException ofe)
            {
                m_AccessOpResult.m_Result = ofe.Result;
                m_AccessOpResult.m_StatusDescription = ofe.StatusDescription;
                m_AccessOpResult.m_VendorMessage = ofe.VendorMessage;
            }
            catch (InvalidUsageException ex)
            {
                m_AccessOpResult.m_Result = RFIDResults.RFID_API_PARAM_ERROR;
                m_AccessOpResult.m_StatusDescription = ex.Info;
                m_AccessOpResult.m_VendorMessage = ex.VendorMessage;
            }
            accessEvent.Result = m_AccessOpResult;
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs pce)
        {
            
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs accessEvents)
        {
            int index = 0;
            if (accessEvents.Error != null)
            {
                functionCallStatusLabel.Text = accessEvents.Error.Message;
            }
            else
            {
                // Handle AccessWait Operations              
                AccessOperationResult accessOpResult = (AccessOperationResult)accessEvents.Result;
                if (accessOpResult.m_Result == RFIDResults.RFID_API_SUCCESS)
                {
                    if (accessOpResult.m_OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ)
                    {
                        if (inventoryList.SelectedItems.Count > 0)
                        {
                            ListViewItem item = inventoryList.SelectedItems[0];
                            string tagID = m_ReadTag.TagID + m_ReadTag.MemoryBank.ToString()
                                + m_ReadTag.MemoryBankDataOffset.ToString();

                            if (item.SubItems[5].Text.Length > 0)
                            {
                                bool isFound = false;

                                // Search or add new one
                                lock (m_TagTable.SyncRoot)
                                {
                                    isFound = m_TagTable.ContainsKey(tagID);
                                }

                                if (!isFound)
                                {
                                    ListViewItem newItem = new ListViewItem(m_ReadTag.TagID);
                                    ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(newItem, m_ReadTag.AntennaID.ToString());
                                    newItem.SubItems.Add(subItem);
                                    subItem = new ListViewItem.ListViewSubItem(item, m_ReadTag.TagSeenCount.ToString());
                                    newItem.SubItems.Add(subItem);
                                    subItem = new ListViewItem.ListViewSubItem(item, m_ReadTag.PeakRSSI.ToString());
                                    newItem.SubItems.Add(subItem);
                                    subItem = new ListViewItem.ListViewSubItem(item, m_ReadTag.PC.ToString("X"));
                                    newItem.SubItems.Add(subItem);
                                    subItem = new ListViewItem.ListViewSubItem(item, m_ReadTag.MemoryBankData);
                                    newItem.SubItems.Add(subItem);

                                    string memoryBank = m_ReadTag.MemoryBank.ToString();
                                    index = memoryBank.LastIndexOf('_');
                                    if (index != -1)
                                    {
                                        memoryBank = memoryBank.Substring(index + 1);
                                    }

                                    subItem = new ListViewItem.ListViewSubItem(item, memoryBank);
                                    newItem.SubItems.Add(subItem);
                                    subItem = new ListViewItem.ListViewSubItem(item, m_ReadTag.MemoryBankDataOffset.ToString());
                                    newItem.SubItems.Add(subItem);

                                    inventoryList.BeginUpdate();
                                    inventoryList.Items.Add(newItem);
                                    inventoryList.EndUpdate();

                                    lock (m_TagTable.SyncRoot)
                                    {
                                        m_TagTable.Add(tagID, newItem);
                                    }
                                }
                            }
                            else
                            {
                                // Empty Memory Bank Slot
                                item.SubItems[5].Text = m_ReadTag.MemoryBankData;

                                string memoryBank = m_ReadForm.m_ReadParams.MemoryBank.ToString();
                                index = memoryBank.LastIndexOf('_');
                                if (index != -1)
                                {
                                    memoryBank = memoryBank.Substring(index + 1);
                                }
                                item.SubItems[6].Text = memoryBank;
                                item.SubItems[7].Text = m_ReadTag.MemoryBankDataOffset.ToString();

                                lock (m_TagTable.SyncRoot)
                                {
                                    if (m_ReadTag.TagID != null)
                                    {
                                        m_TagTable.Remove(m_ReadTag.TagID);
                                    }
                                    m_TagTable.Add(tagID, item);
                                }
                            }
                            this.m_ReadForm.ReadData_TB.Text = m_ReadTag.MemoryBankData;
                            functionCallStatusLabel.Text = "Read Succeed";
                        }
                    }
                    else if (accessOpResult.m_OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE)
                    {
                        functionCallStatusLabel.Text = "Write Succeed";
                    }
                    else if (accessOpResult.m_OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_LOCK)
                    {
                        functionCallStatusLabel.Text = "Lock Succeed";
                    }
                    else if (accessOpResult.m_OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_KILL)
                    {
                        functionCallStatusLabel.Text = "Kill Succeed";
                    }
                    else if (accessOpResult.m_OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_BLOCK_ERASE)
                    {
                        functionCallStatusLabel.Text = "BlockErase Succeed";
                    }
                }
                else
                {
                    functionCallStatusLabel.Text = accessOpResult.m_StatusDescription + " [" + accessOpResult.m_VendorMessage + "]";
                }
                resetButtonState();
            }
        }
    }
}
