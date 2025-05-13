import {
  keepPreviousData,
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";
import {
  getStickerStyles,
  getStickerStyleCount,
  createStickerStyle,
  updateStickerStyle,
  deleteStickerStyle,
  getStickerStyleById,
} from "@/services/sticker-style";

export const useStickerStyles = (page: number, pageSize: number) =>
  useQuery({
    queryKey: ["stickerStyles", page, pageSize],
    queryFn: () => getStickerStyles({ page, pageSize }),
    placeholderData: keepPreviousData,
  });

export const useStickerStyleCount = () =>
  useQuery({
    queryKey: ["stickerStyles", "count"],
    queryFn: getStickerStyleCount,
  });

export const useCreateStickerStyle = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: createStickerStyle,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["stickerStyles", "count"] });
    },
  });
};

export const useUpdateStickerStyle = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: updateStickerStyle,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["stickerStyles"] });
    },
  });
};

export const useDeleteStickerStyle = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: deleteStickerStyle,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["stickerStyles"] });
      queryClient.invalidateQueries({ queryKey: ["count"] });
    },
  });
};

export const useStickerStyleById = (id: number) =>
  useQuery({
    queryKey: ["stickerStyles", id],
    queryFn: () => getStickerStyleById(id),
    enabled: !!id,
  });
