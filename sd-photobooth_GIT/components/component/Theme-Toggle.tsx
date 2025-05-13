"use client";

import React, { useEffect, useState } from "react";
import { useTheme } from "next-themes";
import { ActionIcon, Group } from "@mantine/core";
import { IconMoon } from "@tabler/icons-react";
import { MdWbSunny } from "react-icons/md";

export function ThemeToggle() {
  const { theme, setTheme } = useTheme();
  const [mounted, setMounted] = useState(false);

  useEffect(() => setMounted(true), []);
  if (!mounted) return null;

  return (
    <Group justify="center">
      <ActionIcon
        className="h-10 w-10 rounded-full p-2 bg-white border-gray-300 hover:bg-gray-100 dark:border-gray-700 dark:bg-black dark:hover:bg-gray-700 flex items-center justify-center"
        aria-label="Toggle color scheme"
        onClick={() => setTheme(theme === "light" ? "dark" : "light")}
      >
        {theme === "light" ? (
          <MdWbSunny className="text-gray-700" />
        ) : (
          <IconMoon stroke={1.5} className="text-gray-400" />
        )}
      </ActionIcon>
    </Group>
  );
}