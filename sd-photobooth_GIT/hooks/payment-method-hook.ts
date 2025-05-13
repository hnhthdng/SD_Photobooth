import { keepPreviousData, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  getPaymentMethods,
  getPaymentMethodCount,
  searchPaymentMethodsByName,
  createPaymentMethod,
  updatePaymentMethod,
  deletePaymentMethod,
  getPaymentMethodById,
} from "@/services/payment-method";



export const usePaymentMethods = (page: number, pageSize: number) =>
  useQuery({
    queryKey: ["paymentMethods", page, pageSize],
    queryFn: () => getPaymentMethods({ page, pageSize }),
    placeholderData: keepPreviousData,
  });


export const useSearchPaymentMethods = (name: string) =>
  useQuery({
    queryKey: ["paymentMethods", name],
    queryFn: () => searchPaymentMethodsByName(name),
    enabled: !!name, 
  });


export const usePaymentMethodCount = () =>
  useQuery({
    queryKey: ["paymentMethods","count"],
    queryFn: getPaymentMethodCount,
  });


export const useCreatePaymentMethod = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: createPaymentMethod,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["paymentMethods","count"] });
     
    },
  });
};

export const useUpdatePaymentMethod = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: updatePaymentMethod,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["paymentMethods"] });
    },
  });
};

export const useDeletePaymentMethod = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: deletePaymentMethod,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["paymentMethods"] });
      queryClient.invalidateQueries({ queryKey: ["count"] });
    },
  });
};

export const usePaymentMethodById = (id: number) =>
  useQuery({
    queryKey: ["paymentMethods", id],
    queryFn: () => getPaymentMethodById(id),
    enabled: !!id,
  });
