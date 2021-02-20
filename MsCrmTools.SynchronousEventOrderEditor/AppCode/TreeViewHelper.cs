using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace MsCrmTools.SynchronousEventOrderEditor.AppCode
{
    internal class TreeViewHelper
    {
        private readonly TreeView tv;
        private readonly bool loadInternalEvents;

        public TreeViewHelper(TreeView tv, bool loadInternalEvents)
        {
            this.tv = tv;
            this.loadInternalEvents = loadInternalEvents;
            tv.Sorted = true;
        }

        public void AddSynchronousEvent(ISynchronousEvent sEvent)
        {
            if (!new[] { 10, 20, 40 }.Contains(sEvent.Stage) && !loadInternalEvents)
                return;

            var entityNode = tv.Nodes.Find(sEvent.EntityLogicalName, false).ToList().SingleOrDefault();
            if (entityNode == null)
            {
                entityNode = new TreeNode(sEvent.EntityLogicalName) { ImageIndex = 0, SelectedImageIndex = 0, Name = sEvent.EntityLogicalName };
                tv.Nodes.Add(entityNode);
            }

            var messageNode = entityNode.Nodes.Find(sEvent.Message, false).ToList().SingleOrDefault();
            if (messageNode == null)
            {
                messageNode = new TreeNode(sEvent.Message) { ImageIndex = 1, SelectedImageIndex = 1, Name = sEvent.Message };
                entityNode.Nodes.Add(messageNode);
            }

            var stageNode = messageNode.Nodes.Find(sEvent.Stage.ToString(CultureInfo.InvariantCulture), false).ToList().SingleOrDefault();
            if (stageNode == null)
            {
                string stageName;
                switch (sEvent.Stage)
                {
                    case 5:
                        stageName = "Initial Pre-operation(For internal use only)";
                        break;

                    case 10:
                        stageName = "PreValidation";
                        break;

                    case 20:
                        stageName = "PreOperation";
                        break;

                    case 15:
                        stageName = "Internal Pre-operation Before External Plugins(For internal use only)";
                        break;

                    case 25:
                        stageName = "Internal Pre-operation After External Plugins(For internal use only)";
                        break;

                    case 30:
                        stageName = "Main Operation(For internal use only)";
                        break;

                    case 35:
                        stageName = "Internal Post-operation Before External Plugins(For internal use only)";
                        break;

                    case 40:
                        stageName = "PostOperation";
                        break;

                    case 45:
                        stageName = "Internal Post-operation After External Plugins(For internal use only)";
                        break;

                    case 50:
                        stageName = "Post-operation(Deprecated)";
                        break;

                    case 55:
                        stageName = "Final Post-operation(For internal use only)";
                        break;

                    case 80:
                        stageName = "Pre-Commit stage fired before transaction commit(For internal use only)";
                        break;

                    case 90:
                        stageName = "Post-Commit stage fired after transaction commit(For internal use only)";
                        break;

                    default:
                        stageName = sEvent.Type;
                        break;
                }
                stageNode = new TreeNode(stageName)
                {
                    ImageIndex = 2,
                    SelectedImageIndex = 2,
                    Name = sEvent.Stage.ToString(CultureInfo.InvariantCulture),
                    Tag = new List<ISynchronousEvent>()
                };
                messageNode.Nodes.Add(stageNode);
            }

            ((List<ISynchronousEvent>)stageNode.Tag).Add(sEvent);
        }
    }
}