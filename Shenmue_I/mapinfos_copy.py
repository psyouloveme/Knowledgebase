#!/usr/bin/python3
import os
import os.path
import shutil
import sys

def main():
  SM_SCENE_PATH = sys.argv[1]
  UNITY_ASSETS_PATH = sys.argv[2]
  if not os.path.exists(SM_SCENE_PATH):
    raise Exception('Scene path does not exist')

  if not os.path.exists(UNITY_ASSETS_PATH):
    raise Exception('Assets path does not exist')
  
  # the scene dir should have a bunch of numeric folders
  dirs_to_scan = []
  with os.scandir(SM_SCENE_PATH) as scene_dir:
      for entry in scene_dir:
          if not entry.name.startswith('.') and entry.is_dir():
              dirs_to_scan.append(entry.path)
        
  # find sub dirs of the scene folder - these are where the mapinfos are
  # and have alphanumeric names
  scenes_to_scan = []
  for dir_to_scan in dirs_to_scan:
    with os.scandir(dir_to_scan) as scene_sbudir:
      for entry in scene_sbudir:
        if not entry.name.startswith('.') and entry.is_dir():
          scenes_to_scan.append(entry.path)
  
  # for all of the found subdirs, look for mapinfo.bin files
  # copy them to the destination directory
  for scene in scenes_to_scan:
    with os.scandir(scene) as scene_files:
      for entry in scene_files:
        if entry.is_file() and entry.name.upper() == 'MAPINFO.BIN':
          scene_code = os.path.basename(scene)
          new_filename = scene_code + '_' + entry.name + '.bytes'
          dest_path = os.path.join(UNITY_ASSETS_PATH, new_filename)
          print('copying ' + entry.path + ' to ' + dest_path)
          shutil.copy2(entry.path, dest_path)
          


if __name__ == '__main__':
  main()