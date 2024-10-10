using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IFeedbackService
    {
        List<FeedbackViewModel> GetListOfFeedbacks();

        void AddFeedback(FeedbackViewModel model);

        void EditFeedback(FeedbackViewModel model);

        void DeleteFeedback(int feedbackId);
    }
}