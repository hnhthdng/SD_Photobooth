using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Utils
{
    public static class QueueRoutingHelper
    {
        public static string GetQueueName(string? controlnets)
        {
            if (string.IsNullOrEmpty(controlnets))
                return "image_processing_queue";

            var nets = controlnets.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(n => n.Trim().ToLower())
                                  .ToHashSet();

            return nets switch
            {
                var n when n.SetEquals(new[] { "pose", "canny" }) => "image_processing_queue_openpose_canny",
                var n when n.SetEquals(new[] { "canny", "depth" }) => "image_processing_queue_canny_depth",
                var n when n.SetEquals(new[] { "pose", "depth" }) => "image_processing_queue_openpose_depth",
                _ => "image_processing_queue_openpose_canny"
            };
        }
    }
}
