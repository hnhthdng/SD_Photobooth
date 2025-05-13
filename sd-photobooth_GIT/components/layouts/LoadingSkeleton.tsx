export const LoadingSkeleton = () => {
  return (
    <div className="animate-pulse space-y-4">
      <div className="h-6 bg-gray-300 rounded w-1/3" />
      <div className="h-10 bg-gray-200 rounded" />
      <div className="h-64 bg-gray-200 rounded" />
    </div>
  );
};
