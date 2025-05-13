"use client";

import { ThemeToggle } from "@/components/component/Theme-Toggle";


import ButtonLanguage from "./ButtonLanguage";
import AvatarChildSideBar from "./AvatarChildSideBar";
const Navbar = () => {

  return (



    <div className="flex justify-between items-center gap-5">
      <ThemeToggle />
      <ButtonLanguage />
      <AvatarChildSideBar />
    </div>

  );
};

export default Navbar;
