"use client";
import { useState } from 'react';
import { toast } from 'react-toastify';
import { Button, TextInput, Stack } from '@mantine/core';
import AxiosAPI from '@/configs/axios';

const ChangePassword = () => {
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChangePassword = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await AxiosAPI.post('/api/Identity/change-password', {
        currentPassword,
        newPassword,
      });

      toast.success('Đổi mật khẩu thành công!');
      setCurrentPassword('');
      setNewPassword('');
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Đổi mật khẩu thất bại!');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };
  return (
    <form onSubmit={handleChangePassword}>
      <Stack>
        <TextInput
          label="Mật khẩu hiện tại"
          type="password"
          value={currentPassword}
          onChange={(e) => setCurrentPassword(e.target.value)}
          required
        />
        <TextInput
          label="Mật khẩu mới"
          type="password"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          required
        />
        <Button type="submit" loading={loading}>
          Đổi mật khẩu
        </Button>
      </Stack>
    </form>
  )
}

export default ChangePassword
