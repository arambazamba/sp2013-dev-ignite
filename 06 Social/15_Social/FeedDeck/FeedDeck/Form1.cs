using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Microfeed;
using Microsoft.SharePoint.Client.UserProfiles;

namespace FeedDeck
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        ClientContext clientContext;
        MicrofeedManager microfeedMgr;
        Dictionary<int, string> idDictionary;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clientContext = new ClientContext("http://intranet.contoso.com");
        }

        private void getActivities_Click(object sender, EventArgs e)
        {
            LoadThreads();
        }

        private void postReply_Click(object sender, EventArgs e)
        {
            try
            {
                string threadId = string.Empty;

                //Get the thread identifier
                idDictionary.TryGetValue((int)(threadCount.Value - 1), out threadId);

                // Define properties for the reply.
                MicrofeedPostOptions postOptions = new MicrofeedPostOptions();
                postOptions.Content = responsePost.Text;

                // Register the reply.
                microfeedMgr.PostReply(threadId, postOptions);

                // Make the changes on the server.
                clientContext.ExecuteQuery();

                MessageBox.Show("Reply Posted!");
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void LoadThreads()
        {
            try
            {

                feedThreads.Text = string.Empty;
                string targetUser = accountName.Text;

                // Get the MicrofeedManager object.
                microfeedMgr = new MicrofeedManager(clientContext);

                // Get the properties for the target user
                PersonProperties personProps =
                    new PeopleManager(clientContext).GetPropertiesFor(targetUser);

                // Get Display Name and Account Name for the target user
                clientContext.Load(personProps, o => o.DisplayName, o => o.AccountName);
                clientContext.Load(microfeedMgr);
                clientContext.ExecuteQuery();

                // Specify the feed content that you want to retrieve.
                MicrofeedRetrievalOptions retrievalOptions = new MicrofeedRetrievalOptions();
                retrievalOptions.IncludedTypes = MicroBlogType.RootPost;
                retrievalOptions.ThreadCount = 5;

                // Get all of the target owner's posts and activities
                ClientResult<MicrofeedThreadCollection> threads =
                    microfeedMgr.GetPublishedFeed(
                    personProps.AccountName,
                    retrievalOptions,
                    MicrofeedPublishedFeedType.Full);
                clientContext.ExecuteQuery();

                //Create a dictionary to store the thread identifiers
                idDictionary = new Dictionary<int, string>();

                for (int i = 0; i < threads.Value.Count; i++)
                {
                    MicrofeedThread thread = threads.Value[i];

                    // Keep only user-sourced threads (not events).
                    if (thread.DefinitionName == "Microsoft.SharePoint.Microfeed.UserPost")
                    {
                        //Save thread identifier
                        idDictionary.Add(i, thread.Identifier);

                        // Write out the text of the post.
                        feedThreads.Text += (
                            personProps.DisplayName + ": " +
                            (i + 1) + ". " + thread.RootPost.Content + "\r\n");
                    }
                }

                threadCount.Maximum = threads.Value.Count;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }


    }
}
